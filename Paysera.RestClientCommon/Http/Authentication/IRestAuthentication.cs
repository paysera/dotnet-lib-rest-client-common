using RestSharp;

namespace Paysera.RestClientCommon.Http.Authentication
{
    public interface IRestAuthentication
    {
        IRestClient AddClientAuthentication(IRestClient client);
        IRestRequest AddRequestAuthentication(IRestRequest request);
    }
}
