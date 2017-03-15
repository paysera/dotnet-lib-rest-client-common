using System.Security.Cryptography.X509Certificates;
using Paysera.RestClientCommon.Http.Util.Certificate;

namespace Paysera.RestClientCommon.Http.Util
{
    public class CertificateProvider
    {
        public static X509Certificate2 GetCertificate(string publicCertContents, string privateKeyContents, string password)
        {
            var certBuffer = Helpers.GetBytesFromPem(publicCertContents, PemStringType.Certificate);
            var keyBuffer = Helpers.GetBytesFromPem(privateKeyContents, PemStringType.RsaPrivateKey);
            var certificate = new X509Certificate2(certBuffer, password)
            {
                PrivateKey = Crypto.DecodeRsaPrivateKey(keyBuffer)
            };

            return certificate;
        }
    }
}
