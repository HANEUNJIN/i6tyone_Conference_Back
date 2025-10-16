using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto.Symmetric;
using System.Security.Cryptography;
using System.Text;

namespace eGhis_WebService_Core.Infrastructure.Utils.Crypto
{
    public class CryptoUtil
    {
        #region FILED AREA **************************************************************
        private AesCrypto? _aesCrypto;
        private DesCrypto? _desCrypto;
        #endregion

        #region CONSTRUCTOR AREA ********************************************************
        private CryptoUtil(AesCrypto? aesCrypto = null, DesCrypto? desCrypto = null)
        {
            _aesCrypto = aesCrypto;
            _desCrypto = desCrypto;
        }
        #endregion

        #region GET INSTANCE AREA **********************************************************
        /// <summary>
        /// Crypto field들이 null인 Instance 생성
        /// </summary>
        /// <returns>CryptoUtil Instance <see cref="CryptoUtil"/></returns>
        public static CryptoUtil CreateInstance() => new CryptoUtil();

        /// <summary>
        /// AesCrypto Instance 생성
        /// </summary>
        /// <param name="cryptoKey">AES 암호화의 경우 key가 필요. CTBizConstant.CryptoKey 참조. <see cref="string"/></param>
        /// <param name="iv">초기화 벡터 <see cref="byte"/></param>
        /// <returns>CryptoUtil Instance <see cref="CryptoUtil"/></returns>
        public static CryptoUtil? CreateAESInstance(string cryptoKey = CTBizConstant.CryptoKey.DEFAULT, byte[]? iv = null)
        {
            CryptoUtil? cryptoUtil = null;

            if (string.IsNullOrEmpty(cryptoKey) == false)
            {
                byte[] byteKey = Encoding.UTF8.GetBytes(cryptoKey);
                iv ??= [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

                cryptoUtil = new CryptoUtil(new AesCrypto(256, 128, byteKey, iv));
            }

            return cryptoUtil;
        }

        /// <summary>
        /// DesCrypto Instance 생성
        /// </summary>
        /// <returns>CryptoUtil Instance <see cref="CryptoUtil"/></returns>
        public static CryptoUtil CreateDESInstance() => new CryptoUtil(null, new DesCrypto());
        #endregion

        #region AES CIPHER AREA *********************************************************************
        /// <summary>
        /// AES 암호화
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string? AESEncrypt(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return _aesCrypto?.AESEncrypt(value);
            }

            return value;
        }

        /// <summary>
        /// AES 복호화
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string? AESDecrypt(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return _aesCrypto?.AESDecrypt(value);
            }

            return value;
        }
        #endregion

        #region DES CIPHER AREA ***********************************************************
        public string? DESEncrypt(string value) => _desCrypto?.DESEncrypt(value);

        public string? DESDecrypt(string value) => _desCrypto?.DESDecrypt(value);

        public static string? DESDecryptContext(string? contextValue)
        {
            string? result = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(contextValue) == false)
                    result = CreateDESInstance().DESDecrypt(contextValue);
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Encrypt_Error, $"value: {contextValue}");
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Encrypt_Error, ex.ToString());
                throw new InvalidOperationException("Failed to decrypt the connection string.", ex);
            }

            return result;
        }
        #endregion

        #region CRYPTO HASH FUNCTION AREA ************************************************
        /// <summary>
        /// sha256암호화
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public byte[] SHA256Encrypt(string str) => SHA256.HashData(Encoding.UTF8.GetBytes(str));

        /// <summary>
        /// SHA-256 암호화
        /// </summary>
        /// <param name="source">원본 문자열</param>
        /// <param name="salt">SALT 값</param>
        /// <returns>암호화된 문자열</returns>
        public static string SHA256Encrypt(string source, string salt) => SHA256Encrypt(source, Encoding.UTF8.GetBytes(salt));

        public string SHA256SaltHash(string plainText, string saltText)
        {
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] saltBytes = Encoding.ASCII.GetBytes(saltText);

            byte[] plainWithSaltBytes = plainBytes.Concat(saltBytes).ToArray();

            byte[] hashBytes = SHA256.HashData(plainWithSaltBytes);

            StringBuilder sb = new StringBuilder(hashBytes.Length * 2 + hashBytes.Length / 8);
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(BitConverter.ToString(hashBytes, i, 1));
            }

            string returnKey = sb.ToString().TrimEnd(new char[] { ' ' }).ToLower();

            return returnKey;
        }

        /// <summary>
        /// SHA-256 암호화
        /// </summary>
        /// <param name="source">원본 문자열</param>
        /// <param name="salt">SALT 값 (바이트 배열)</param>
        /// <returns>암호화된 문자열</returns>
        private static string SHA256Encrypt(string source, byte[] salt)
        {
            string result = string.Empty;

            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            byte[] combinedBytes = new byte[sourceBytes.Length + salt.Length];

            Buffer.BlockCopy(sourceBytes, 0, combinedBytes, 0, sourceBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, sourceBytes.Length, salt.Length);

            try
            {
                byte[] hashBytes = SHA256.HashData(combinedBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                result = sb.ToString();
            }
            catch (Exception e)
            {
                // 예외 처리 (필요에 따라 처리 로직 추가)
                Console.WriteLine(e.Message);
            }

            return result;
        }
        #endregion
    }
}
