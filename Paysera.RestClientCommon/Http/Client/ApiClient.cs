using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Paysera.RestClientCommon.Http.ExceptionMapper;
using Paysera.RestClientCommon.Http.Task;
using RestSharp;

namespace Paysera.RestClientCommon.Http.Client
{
    public class ApiClient
    {
        private const string ContentTypeJson = "application/json";

        private readonly IRestClient _restClient;
        private readonly IExceptionMapper _exceptionMapper;

        public ApiClient(IRestClient restClient, IExceptionMapper exceptionMapper)
        {
            _restClient = restClient;
            _exceptionMapper = exceptionMapper;
        }

        public ApiTask<TResult> PutAsync<TResult, TPayload>(string resource, TPayload payload = default(TPayload))
        {
            var request = BuildRequest(resource);
            request.Method = Method.PUT;
            request.RequestFormat = DataFormat.Json;
            request = AddPayload(request, payload);

            return ExecuteRequestAndMap<TResult>(request);
        }

        public ApiTask<TResult> PostAsync<TResult, TPayload>(string resource, TPayload payload = default(TPayload))
        {
            var request = BuildRequest(resource);
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request = AddPayload(request, payload);

            return ExecuteRequestAndMap<TResult>(request);
        }

        public ApiTask<TResult> GetAsync<TResult>(string resource)
        {
            var request = BuildRequest(resource);
            return ExecuteRequestAndMap<TResult>(request);
        }

        public Task<IRestResponse> GetTaskPlain(string resource)
        {
            var request = BuildRequest(resource);
            return _restClient.ExecuteGetTaskAsync(request);
        }

        private ApiTask<T> ExecuteRequestAndMap<T>(IRestRequest request)
        {
            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            var tcs = new TaskCompletionSource<IRestResponse>();

            _restClient.ExecuteAsync(request, tcs.SetResult);
            var requestAndMapTask = tcs.Task.ContinueWith((previousTask) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return MapToEntity<T>(previousTask.Result);

            }, cancellationToken, TaskContinuationOptions.LongRunning, TaskScheduler.Default);

            return ApiTask<T>.Create(requestAndMapTask, tokenSource);
        }

        private T MapToEntity<T>(IRestResponse response)
        {
            if (
                response.StatusCode == HttpStatusCode.OK
                && response.ContentType.ToLower() == ContentTypeJson
            )
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }

            throw _exceptionMapper.MapFromResponse(response);
        }

        private IRestRequest BuildRequest(string resource)
        {
            return new RestRequest(resource);
        }

        private IRestRequest AddPayload<T>(IRestRequest request, T payload)
        {
            if (request == null) return null;

            var serializedPayload = JsonConvert.SerializeObject(payload);
            request.AddParameter(ContentTypeJson, serializedPayload, ParameterType.RequestBody);

            return request;
        }
    }
}
