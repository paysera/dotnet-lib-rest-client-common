using System.Security.Cryptography.X509Certificates;
using Paysera.RestClientCommon.Http.Util;
using RestSharp;

namespace Paysera.RestClientCommon.Http.Authentication
{
    public class ClientCertificateAuthentication : IRestAuthentication
    {
        private readonly string _certificate;
        private readonly string _privateKey;
        private readonly string _password;

        public ClientCertificateAuthentication(string certificateFilePath, string privateKeyFilePath)
            : this(certificateFilePath, privateKeyFilePath, string.Empty)
        {
        }

        public ClientCertificateAuthentication(string certificateFilePath, string privateKeyFilePath, string password)
        {
            _certificate = System.IO.File.ReadAllText(certificateFilePath);
            _privateKey = System.IO.File.ReadAllText(privateKeyFilePath);
            _password = password;
        }

        public IRestClient AddClientAuthentication(IRestClient client)
        {
            if (client.ClientCertificates == null)
            {
                client.ClientCertificates = new X509CertificateCollection();
            }

            var clientCertificate = CertificateProvider.GetCertificate(_certificate, _privateKey, _password);
            client.ClientCertificates.Add(clientCertificate);

            return client;
        }

        public IRestRequest AddRequestAuthentication(IRestRequest request)
        {
            // No additional HTTP headers for this type of authentication
            return request;
        }
    }
}
