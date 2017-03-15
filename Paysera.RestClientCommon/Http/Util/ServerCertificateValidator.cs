using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Paysera.RestClientCommon.Http.Util
{
    public class ServerCertificateValidator
    {
        private readonly string _serverCertificateFilePath;

        public ServerCertificateValidator(string serverCertificateFilePath)
        {
            _serverCertificateFilePath = serverCertificateFilePath;
        }

        public bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None || GetLocalCertificate().GetCertHash().SequenceEqual(certificate.GetCertHash());
        }

        private X509Certificate2 GetLocalCertificate()
        {
            var cert = new X509Certificate2();
            cert.Import(_serverCertificateFilePath);

            return cert;
        }
    }
}
