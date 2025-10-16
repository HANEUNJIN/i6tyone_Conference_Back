using System.Security.Cryptography;
using System.Text;
using eGhis_WebService_Core.Exceptions;
using eGhis_WebService_Core.Infrastructure.Utils.Codec;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto.Symmetric.Base;

namespace eGhis_WebService_Core.Infrastructure.Utils.Crypto.Symmetric
{
    /// <summary>
    /// DES 암/복호화 클래스
    /// </summary>
    internal class DesCrypto : BaseCryptoSymmetric
    {
        #region FIELD AREA ******************************************************
        private static readonly string _password = "eghis1234@!";
        #endregion

        #region CONSTRUCTOR AREA ****************************************************
        /// <summary>
        /// 생성자
        /// </summary>
        public DesCrypto()
            : base(64, 64)
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="key">암호화 키</param>
        /// <param name="iv">초기화 벡터</param>
        public DesCrypto(string key, string iv)
            : base(64, 64, key, iv)
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="key">암호화 키</param>
        /// <param name="iv">초기화 벡터</param>
        public DesCrypto(byte[] key, byte[] iv)
            : base(64, 64, key, iv)
        {
        }
        #endregion

        #region OVERRIDE METHOD AREA ***************************************************************
        /// <summary>
        /// 신규 비밀 키를 반환합니다.
        /// </summary>
        /// <returns>비밀 키</returns>
        public override byte[] GenerateKey()
        {
            using DES des = DES.Create();

            des.KeySize = KeySize;
            des.GenerateKey();

            return des.Key;
        }

        /// <summary>
        /// 신규 초기화 벡터를 반환합니다.
        /// </summary>
        /// <returns>초기화 벡터</returns>
        public override byte[] GenerateIV()
        {
            using DES des = DES.Create();

            des.BlockSize = BlockSize;
            des.GenerateIV();

            return des.IV;
        }

        /// <summary>
        /// 입력된 평문을 암호문으로 변환합니다.
        /// </summary>
        /// <param name="plainText">평문</param>
        /// <returns>암호문</returns>
        public override string AESEncrypt(string plainText)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(plainText) == false)
            {
                try
                {
                    byte[]? cipherValue = null;

                    using (DES crypto = DES.Create())
                    {
                        crypto.Mode = (CipherMode)CipherMode;
                        crypto.Padding = (PaddingMode)PaddingMode;
                        crypto.KeySize = KeySize;
                        crypto.BlockSize = BlockSize;
                        crypto.Key = Key;
                        crypto.IV = IV;

                        using (ICryptoTransform transform = crypto.CreateEncryptor())
                        {
                            cipherValue = transform.TransformFinalBlock(TextEncoder.GetBytes(plainText), 0, TextEncoder.GetByteCount(plainText));
                        }
                    }

                    result = EncodingUtil.ToEncodedCryptoString(EncodingType, cipherValue);
                }
                catch (Exception ex)
                {
                    throw new CryptoException(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 입력된 암호문을 평문으로 변환합니다.
        /// </summary>
        /// <param name="cipherText">암호문</param>
        /// <returns>평문</returns>
        public override string AESDecrypt(string cipherText)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(cipherText) == false)
            {
                try
                {
                    byte[]? plainValue = null;

                    using (DES crypto = DES.Create())
                    {
                        crypto.Mode = (CipherMode)CipherMode;
                        crypto.Padding = (PaddingMode)PaddingMode;
                        crypto.KeySize = KeySize;
                        crypto.BlockSize = BlockSize;
                        crypto.Key = Key;
                        crypto.IV = IV;

                        using (ICryptoTransform transform = crypto.CreateDecryptor())
                        {
                            byte[] cipherValue = EncodingUtil.FromEncodedCryptoString(EncodingType, cipherText);
                            plainValue = transform.TransformFinalBlock(cipherValue, 0, cipherValue.Length);
                        }
                    }

                    result = TextEncoder.GetString(plainValue);
                }
                catch (Exception ex)
                {
                    throw new CryptoException(ex);
                }
            }

            return result;
        }
        #endregion

        #region GENERAL METHOD AREA ************************************************************
        public string DESEncrypt(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] salt = new byte[8];
            new Random().NextBytes(salt);

            FindKeyAndIV(salt, _password, out byte[] key, out byte[] iv);

            using DES des = DES.Create();
            byte[] resultBytes = des.CreateEncryptor(key, iv).TransformFinalBlock(textBytes, 0, textBytes.Length);
            byte[] encryptedBytes = new byte[resultBytes.Length + salt.Length];
            Array.Copy(salt, encryptedBytes, salt.Length);
            Array.Copy(resultBytes, 0, encryptedBytes, salt.Length, resultBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        public string DESDecrypt(string text)
        {
            byte[] textBytes = Convert.FromBase64String(text);

            byte[] salt = new byte[8];
            byte[] encryptedBytes = new byte[textBytes.Length - salt.Length];
            Array.Copy(textBytes, salt, salt.Length);
            Array.Copy(textBytes, salt.Length, encryptedBytes, 0, encryptedBytes.Length);

            FindKeyAndIV(salt, _password, out byte[] key, out byte[] iv);

            using DES des = DES.Create();
            byte[] decryptedBytes = des.CreateDecryptor(key, iv).TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
        #endregion

        #region INTERNAL METHOD AREA ************************************************************
        private void FindKeyAndIV(byte[] salt, string password, out byte[] key, out byte[] iv)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, hash, passwordBytes.Length);
            Array.Copy(salt, 0, hash, passwordBytes.Length, salt.Length);

            using (MD5 md5 = MD5.Create())
            {
                //md5 iterations: 1000
                for (int i = 0; i < 1000; i++)
                    hash = md5.ComputeHash(hash);
            }

            key = new byte[8];
            iv = new byte[8];
            Array.Copy(hash, 0, key, 0, 8);
            Array.Copy(hash, 8, iv, 0, 8);
        }
        #endregion
    }
}
