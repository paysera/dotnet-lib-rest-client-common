using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Paysera.RestClientCommon.Http.Util.Certificate
{
    public class Certificate
    {
        public Certificate() { }

        public Certificate(string cert, string key, string password)
        {
            PublicCertificate = cert;
            PrivateKey = key;
            Password = password;
        }

        private string PublicCertificate { get; }

        private string PrivateKey { get; }

        private string Password { get; }

        private static X509Certificate2 GetCertificateFromPemString(string publicCert)
        {
            return new X509Certificate2(Encoding.UTF8.GetBytes(publicCert));
        }

        private static X509Certificate2 GetCertificateFromPemString(string publicCert, string privateKey, string password)
        {
            var certBuffer = Helpers.GetBytesFromPem(publicCert, PemStringType.Certificate);
            var keyBuffer  = Helpers.GetBytesFromPem(privateKey, PemStringType.RsaPrivateKey);

            var certificate = new X509Certificate2(certBuffer, password)
            {
                PrivateKey = Crypto.DecodeRsaPrivateKey(keyBuffer)
            };

            return certificate;
        }

    }
}
