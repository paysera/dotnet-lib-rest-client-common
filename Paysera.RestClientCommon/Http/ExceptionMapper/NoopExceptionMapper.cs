using System;
using RestSharp;

namespace Paysera.RestClientCommon.Http.ExceptionMapper
{
    public class NoopExceptionMapper : IExceptionMapper
    {
        public Exception MapFromResponse(IRestResponse response)
        {
            return response.ErrorException ?? new Exception($"{response.StatusCode}: {response.ResponseUri.AbsoluteUri}");
        }
    }
}
