using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace eGhis_WebService_Core.Infrastructure.Utils.Codec
{
    public class EncodingUtil
    {
        #region CONSTRUCTOR AREA ********************************************************
        private EncodingUtil()
        {
        }
        #endregion

        #region GENERAL BASE64 ENCODE & DECODE AREA ****************************************************
        public string Base64Encode(string str)
        {
            return Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(str));
        }

        public string Base64Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public string Base64Decode(string str)
        {
            return Encoding.GetEncoding("utf-8").GetString(Convert.FromBase64String(str));
        }

        /// <summary>
		/// 랜덤 문자열 생성
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public string RandomDataBase64(uint length)
        {
            byte[] bytes = new byte[length];

            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }

            return Base64UrlEncode(bytes);
        }

        public string Base64UrlEncode(string str)
        {
            return Base64UrlEncoder.Encode(str);
        }

        public string Base64UrlEncode(byte[] bytes)
        {
            return Base64UrlEncoder.Encode(bytes);
        }

        public string Base64UrlDecode(string str)
        {
            return Base64UrlEncoder.Decode(str);
        }

        /// <summary>
        /// base64urlencode(sha256)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Sha256EncryptToBase64Url(string str)
        {
            return Base64UrlEncode(CryptoUtil.CreateInstance().SHA256Encrypt(str));
        }
        #endregion

        #region GENERAL STATIC ENCODE & DECODE AREA ****************************************************

        /// <summary>
        /// Base64 인코딩된 문자열을 바이트 배열로 변환합니다.
        /// </summary>
        /// <param name="str">Base64 인코딩된 문자열</param>
        /// <returns>바이트 배열</returns>
        public static byte[] FromBase64(string str)
        {
            return Convert.FromBase64String(str);
        }

        /// <summary>
        /// 바이트 배열을 Base64 인코딩된 문자열로 변환합니다.
        /// </summary>
        /// <param name="bytes">바이트 배열</param>
        /// <returns>Base64 인코딩된 문자열</returns>
        public static string ToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 바이트 배열을 Base64 인코딩된 문자열로 변환합니다.
        /// </summary>
        /// <param name="bytes">바이트 배열</param>
        /// <param name="index">변환 할 첫 번째 바이트 인덱스</param>
        /// <param name="count">변환 할 바이트 수</param>
        /// <returns>Base64 인코딩된 문자열</returns>
        public static string ToBase64(byte[] bytes, int index, int count)
        {
            return Convert.ToBase64String(bytes, index, count);
        }

        /// <summary>
        /// HEX 인코딩된 문자열을 바이트 배열로 변환합니다.
        /// </summary>
        /// <param name="str">HEX 인코딩된 문자열</param>
        /// <returns>바이트 배열</returns>
        public static byte[] FromHex(string str)
        {
            List<byte> bytes = new List<byte>();

            for (int i = 0; i * 2 < str.Length; i++)
            {
                byte ch = Convert.ToByte(str.Substring(i * 2, 2), 16);
                bytes.Add(ch);
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// HEX 인코딩된 바이트를 문자로 변환합니다.
        /// </summary>
        /// <param name="b">바이트</param>
        /// <returns>HEX 인코딩된 문자</returns>
        public static string ToHex(byte b)
        {
            return string.Format("{0:X2}", b);
        }

        /// <summary>
        /// 바이트 배열을 HEX 인코딩된 문자열로 변환합니다.
        /// </summary>
        /// <param name="bytes">바이트 배열</param>
        /// <returns>HEX 인코딩된 문자열</returns>
        public static string ToHex(byte[] bytes)
        {
            return ToHex(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 바이트 배열을 HEX 인코딩된 문자열로 변환합니다.
        /// </summary>
        /// <param name="bytes">바이트 배열</param>
        /// <param name="index">변환 할 첫 번째 바이트 인덱스</param>
        /// <param name="count">변환 할 바이트 수</param>
        /// <returns>HEX 인코딩된 문자열</returns>
        public static string ToHex(byte[] bytes, int index, int count)
        {
            StringBuilder str = new StringBuilder();

            for (int i = index; i < index + count; i++)
            {
                str.Append(ToHex(bytes[i]));
            }

            return str.ToString();
        }

        /// <summary>
        /// 인코딩된 문자열을 바이트 배열로 변환합니다.
        /// </summary>
        /// <param name="stringEncodingType">문자열 인코딩 타입</param>
        /// <param name="str">인코딩된 문자열</param>
        /// <returns>바이트 배열</returns>
        public static byte[] FromEncodedCryptoString(EEncodingTypes stringEncodingType, string str)
        {
            if (stringEncodingType == EEncodingTypes.HEX)
            {
                return FromHex(str);
            }
            else
            {
                return FromBase64(str);
            }
        }

        /// <summary>
        /// 바이트 배열을 인코딩된 문자열로 변환합니다.
        /// </summary>
        /// <param name="stringEncodingType">문자열 인코딩 타입</param>
        /// <param name="bytes">바이트 배열</param>
        /// <returns>인코딩된 문자열</returns>
        public static string ToEncodedCryptoString(EEncodingTypes stringEncodingType, byte[] bytes)
        {
            if (stringEncodingType == EEncodingTypes.HEX)
            {
                return ToHex(bytes);
            }
            else
            {
                return ToBase64(bytes);
            }
        }
        #endregion
    }
}
