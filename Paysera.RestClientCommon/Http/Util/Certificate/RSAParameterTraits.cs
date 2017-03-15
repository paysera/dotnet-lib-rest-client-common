using System;

namespace Paysera.RestClientCommon.Http.Util.Certificate
{
    internal class RsaParameterTraits
    {
        public RsaParameterTraits(int modulusLengthInBits)
        {
            int assumedLength;
            var logbase = Math.Log(modulusLengthInBits, 2);
            if (logbase == (int)logbase)
            {
                assumedLength = modulusLengthInBits;
            }
            else
            {
                assumedLength = (int)(logbase + 1.0);
                assumedLength = (int)(Math.Pow(2, assumedLength));
                System.Diagnostics.Debug.Assert(false);
            }

            switch (assumedLength)
            {
                case 1024:
                    SizeMod = 0x80;
                    SizeExp = -1;
                    SizeD = 0x80;
                    SizeP = 0x40;
                    SizeQ = 0x40;
                    SizeDp = 0x40;
                    SizeDq = 0x40;
                    SizeInvQ = 0x40;
                    break;
                case 2048:
                    SizeMod = 0x100;
                    SizeExp = -1;
                    SizeD = 0x100;
                    SizeP = 0x80;
                    SizeQ = 0x80;
                    SizeDp = 0x80;
                    SizeDq = 0x80;
                    SizeInvQ = 0x80;
                    break;
                case 4096:
                    SizeMod = 0x200;
                    SizeExp = -1;
                    SizeD = 0x200;
                    SizeP = 0x100;
                    SizeQ = 0x100;
                    SizeDp = 0x100;
                    SizeDq = 0x100;
                    SizeInvQ = 0x100;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }
        }

        public readonly int SizeMod  = -1;
        public readonly int SizeExp  = -1;
        public readonly int SizeD    = -1;
        public readonly int SizeP    = -1;
        public readonly int SizeQ    = -1;
        public readonly int SizeDp   = -1;
        public readonly int SizeDq   = -1;
        public readonly int SizeInvQ = -1;
    }
}
