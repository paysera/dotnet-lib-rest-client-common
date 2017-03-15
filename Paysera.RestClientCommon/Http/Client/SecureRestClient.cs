using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Paysera.RestClientCommon.Http.Authentication;
using Paysera.RestClientCommon.Http.Util;
using RestSharp;

namespace Paysera.RestClientCommon.Http.Client
{
    public class SecureRestClient : RestClient
    {
        private readonly ServerCertificateValidator _serverCertificateValidator;
        private readonly IRestAuthentication _authentication;
        private bool _certificateCallbackSubscribed;

        public SecureRestClient(string baseUri, IRestAuthentication authentication, ServerCertificateValidator serverCertificateValidator = null)
            : base(baseUri)
        {
            authentication.AddClientAuthentication(this);

            _authentication = authentication;
            _serverCertificateValidator = serverCertificateValidator;
            _certificateCallbackSubscribed = false;
        }

        public override RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            EnforceValidSsl();

            _authentication.AddRequestAuthentication(request);
            return base.ExecuteAsync(request, callback);
        }

        public override Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request)
        {
            EnforceValidSsl();

            _authentication.AddRequestAuthentication(request);
            return base.ExecuteGetTaskAsync(request);
        }

        private void EnforceValidSsl()
        {
            if (_serverCertificateValidator != null)
            {
                if (_certificateCallbackSubscribed) return;

                ServicePointManager.ServerCertificateValidationCallback += _serverCertificateValidator.ServerCertificateValidationCallback;
                _certificateCallbackSubscribed = true;
            }
            else
            {
                if (_certificateCallbackSubscribed) return;

                ServicePointManager.ServerCertificateValidationCallback += NoSslErrorsCallback;
                _certificateCallbackSubscribed = true;
            }
        }

        private bool NoSslErrorsCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}
