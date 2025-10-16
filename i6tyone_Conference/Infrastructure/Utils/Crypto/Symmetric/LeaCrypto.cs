using System.Security.Cryptography;

namespace eGhis_WebService_Core.Infrastructure.Utils.Crypto.Symmetric
{
    internal class LeaCrypto
    {
        #region GENERAL STATIC METHOD AREA *******************************************************
        public static byte[] CtrEncode(byte[] data, byte[] key, byte[] iv)
        {
            byte[] enc = new byte[data.Length];

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                int offset = 0;
                int size = data.Length;

                while (offset < size)
                {
                    int len = iv.Length;
                    if (size - offset < iv.Length)
                    {
                        len = size - offset;
                    }
                    byte[] block = new byte[len];
                    Array.Copy(data, offset, block, 0, len);

                    byte[] ct;
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        ct = encryptor.TransformFinalBlock(block, 0, block.Length);
                    }

                    Array.Copy(ct, 0, enc, offset, ct.Length);
                    offset += ct.Length;
                }
            }

            return enc;
        }

        public static byte[] CtrDecode(byte[] data, byte[] key, byte[] iv)
        {
            byte[] dec = new byte[data.Length];

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                int offset = 0;
                int size = data.Length;

                while (offset < size)
                {
                    int len = iv.Length;
                    if (size - offset < iv.Length)
                    {
                        len = size - offset;
                    }
                    byte[] block = new byte[len];
                    Array.Copy(data, offset, block, 0, len);

                    byte[] pt;
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        pt = decryptor.TransformFinalBlock(block, 0, block.Length);
                    }

                    Array.Copy(pt, 0, dec, offset, pt.Length);
                    offset += pt.Length;
                }
            }

            return dec;
        }
        #endregion
    }
}
