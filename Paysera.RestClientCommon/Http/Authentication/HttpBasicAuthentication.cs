using RestSharp;
using RestSharp.Authenticators;

namespace Paysera.RestClientCommon.Http.Authentication
{
    public class HttpBasicAuthentication : IRestAuthentication
    {
        private readonly HttpBasicAuthenticator _httpBasicAuthenticator;

        public HttpBasicAuthentication(string username, string password)
        {
            _httpBasicAuthenticator = new HttpBasicAuthenticator(username, password);
        }

        public IRestClient AddClientAuthentication(IRestClient client)
        {
            return client;
        }

        public IRestRequest AddRequestAuthentication(IRestRequest request)
        {
            _httpBasicAuthenticator.Authenticate(null, request);
            return request;
        }
    }
}
