using System;

namespace Paysera.RestClientCommon.Http.Util.Certificate
{
    public class Helpers
    {
        public static int DecodeIntegerSize(System.IO.BinaryReader rd)
        {
            int count;

            var byteValue = rd.ReadByte();
            if (byteValue != 0x02)
                return 0;

            byteValue = rd.ReadByte();
            switch (byteValue)
            {
                case 0x81:
                    count = rd.ReadByte();
                    break;
                case 0x82:
                    var hi = rd.ReadByte();
                    var lo = rd.ReadByte();
                    count = BitConverter.ToUInt16(new[] { lo, hi }, 0);
                    break;
                default:
                    count = byteValue;
                    break;
            }

            while (rd.ReadByte() == 0x00)
            {
                count -= 1;
            }
            rd.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);

            return count;
        }

        public static byte[] GetBytesFromPem(string pemString, PemStringType type)
        {
            string header; string footer;

            switch (type)
            {
                case PemStringType.Certificate:
                    header = "-----BEGIN CERTIFICATE-----";
                    footer = "-----END CERTIFICATE-----";
                    break;
                case PemStringType.RsaPrivateKey:
                    header = "-----BEGIN RSA PRIVATE KEY-----";
                    footer = "-----END RSA PRIVATE KEY-----";
                    break;
                default:
                    return null;
            }

            var start = pemString.IndexOf(header, StringComparison.Ordinal) + header.Length;
            var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;

            if (start < 0 || end < 0)
            {
                throw new ArgumentException("Invalid PEM string content: perhaps a certificate is passed instead of the private key or vice versa?");
            }

            return Convert.FromBase64String(pemString.Substring(start, end));
        }

        public static byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            var inputBytesSize = inputBytes.Length;

            if ((alignSize != -1) && (inputBytesSize < alignSize))
            {
                var buf = new byte[alignSize];
                for (var i = 0; i < inputBytesSize; ++i)
                {
                    buf[i + (alignSize - inputBytesSize)] = inputBytes[i];
                }
                return buf;
            }
            else
            {
                return inputBytes;
            }
        }
    }
}
