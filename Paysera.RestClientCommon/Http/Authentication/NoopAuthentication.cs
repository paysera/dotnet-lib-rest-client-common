using RestSharp;

namespace Paysera.RestClientCommon.Http.Authentication
{
    public class NoopAuthentication : IRestAuthentication
    {
        public IRestClient AddClientAuthentication(IRestClient client)
        {
            return client;
        }

        public IRestRequest AddRequestAuthentication(IRestRequest request)
        {
            return request;
        }
    }
}
