using System;
using System.IO;
using System.Security.Cryptography;

namespace Paysera.RestClientCommon.Http.Util.Certificate
{
    public class Crypto
    {
        public static RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privateKeyBytes)
        {
            var rd = new BinaryReader(new MemoryStream(privateKeyBytes));

            try
            {
                var shortValue = rd.ReadUInt16();

                switch (shortValue)
                {
                    case 0x8130:
                        rd.ReadByte();
                        break;
                    case 0x8230:
                        rd.ReadInt16();
                        break;
                    default:
                        return null;
                }

                shortValue = rd.ReadUInt16();
                if (shortValue != 0x0102)
                {
                    return null;
                }

                var byteValue = rd.ReadByte();
                if (byteValue != 0x00)
                {
                    return null;
                }

                // The data following the version will be the ASN.1 data itself, which in our case
                // are a sequence of integers.

                // In order to solve a problem with instancing RSACryptoServiceProvider
                // via default constructor on .net 4.0 this is a hack
                CspParameters parms = new CspParameters
                {
                    Flags = CspProviderFlags.NoFlags,
                    KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant(),
                    ProviderType =
                        ((Environment.OSVersion.Version.Major > 5) ||
                         ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
                            ? 0x18
                            : 1
                };

                var rsa = new RSACryptoServiceProvider(parms);
                var rsAparams = new RSAParameters {Modulus = rd.ReadBytes(Helpers.DecodeIntegerSize(rd))};


                // Argh, this is a pain.  From emperical testing it appears to be that RSAParameters doesn't like byte buffers that
                // have their leading zeros removed.  The RFC doesn't address this area that I can see, so it's hard to say that this
                // is a bug, but it sure would be helpful if it allowed that. So, there's some extra code here that knows what the
                // sizes of the various components are supposed to be.  Using these sizes we can ensure the buffer sizes are exactly
                // what the RSAParameters expect.  Thanks, Microsoft.
                var traits = new RsaParameterTraits(rsAparams.Modulus.Length * 8);

                rsAparams.Modulus = Helpers.AlignBytes(rsAparams.Modulus, traits.SizeMod);
                rsAparams.Exponent = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.SizeExp);
                rsAparams.D = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.SizeD);
                rsAparams.P = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.SizeP);
                rsAparams.Q = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.SizeQ);
                rsAparams.DP = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.SizeDp);
                rsAparams.DQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.SizeDq);
                rsAparams.InverseQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.SizeInvQ);

                rsa.ImportParameters(rsAparams);
                return rsa;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                rd.Close();
            }
        }
    }
}
