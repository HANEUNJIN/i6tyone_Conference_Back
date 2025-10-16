using System.Security.Cryptography;
using eGhis_WebService_Core.Exceptions;
using eGhis_WebService_Core.Infrastructure.Utils.Codec;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto.Symmetric.Base;

namespace eGhis_WebService_Core.Infrastructure.Utils.Crypto.Symmetric
{
    /// <summary>
    /// Triple DES 암/복호화 클래스
    /// </summary>
    internal class TripleDesCrypto : BaseCryptoSymmetric
    {
        #region PROPERTY AREA *********************************************************
        /// <summary>
        /// 암호화 키 크기
        /// </summary>
        public override int KeySize
        {
            get
            {
                return base.KeySize;
            }
            protected set
            {
                if (value != 128 && value != 192)
                {
                    throw new CryptoException("키 크기가 잘못되었습니다.(128/192 중 선택)");
                }

                base.KeySize = value;
            }
        }
        #endregion

        #region CONSTRUCTOR AREA *********************************************************
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="keySize">암호화 키 크기</param>
        public TripleDesCrypto(int keySize)
            : base(keySize, 64)
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="keySize">암호화 키 크기</param>
        /// <param name="key">암호화 키</param>
        /// <param name="iv">초기화 벡터</param>
        public TripleDesCrypto(int keySize, string key, string iv)
            : base(keySize, 64, key, iv)
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="keySize">암호화 키 크기</param>
        /// <param name="key">암호화 키</param>
        /// <param name="iv">초기화 벡터</param>
        public TripleDesCrypto(int keySize, byte[] key, byte[] iv)
            : base(keySize, 64, key, iv)
        {
        }
        #endregion

        #region OVERRIDE METHOD AREA *************************************************************
        /// <summary>
        /// 신규 비밀 키를 반환합니다.
        /// </summary>
        /// <returns>비밀 키</returns>
        public override byte[] GenerateKey()
        {
            using TripleDES tdes = TripleDES.Create();

            tdes.KeySize = KeySize;
            tdes.GenerateKey();
          
            return tdes.Key;
        }

        /// <summary>
        /// 신규 초기화 벡터를 반환합니다.
        /// </summary>
        /// <returns>초기화 벡터</returns>
        public override byte[] GenerateIV()
        {
            using TripleDES tdes = TripleDES.Create();

            tdes.BlockSize = BlockSize;
            tdes.GenerateIV();

            return tdes.IV;
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

                    using (TripleDES crypto = TripleDES.Create())
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

                    using (TripleDES crypto = TripleDES.Create())
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
    }
}
