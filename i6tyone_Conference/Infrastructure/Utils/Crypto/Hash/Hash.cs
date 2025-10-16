using System.Security.Cryptography;
using System.Text;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Utils.Codec;

namespace eGhis_WebService_Core.Infrastructure.Utils.Crypto.Hash
{
    /// <summary>
    /// 해쉬 변환 클래스
    /// </summary>
    internal class Hash
    {
        #region FIELD AREA ************************************************************
        private Encoding _textEncoder = Encoding.Default;
        #endregion

        #region PROPERTY AREA ***************************************************************
        /// <summary>
        /// 텍스트 인코딩 객체를 가져오거나 설정합니다.
        /// </summary>
        public Encoding TextEncoder
        {
            get
            {
                return _textEncoder;
            }
            set
            {
                _textEncoder = value;
            }
        }
        #endregion

        #region GENERAL METHOD AREA **********************************************************************
        /// <summary>
        /// 입력된 문자열을 해쉬 문자열로 변환합니다.
        /// </summary>
        /// <param name="input">문자열</param>
        /// <param name="hashType">해쉬 타입</param>
        /// <param name="encodingType">인코딩 타입</param>
        /// <returns>해쉬 문자열</returns>
        public string TransformHash(string input, EHashTypes hashType, EEncodingTypes encodingType = EEncodingTypes.Default)
        {
            byte[] computedHash;

            switch (hashType)
            {
                case EHashTypes.MD5:
                    computedHash = MD5.HashData(TextEncoder.GetBytes(input));
                    break;

                case EHashTypes.SHA256:
                    computedHash = SHA256.HashData(TextEncoder.GetBytes(input));
                    break;

                case EHashTypes.SHA384:
                    computedHash = SHA384.HashData(TextEncoder.GetBytes(input));
                    break;

                case EHashTypes.SHA512:
                    computedHash = SHA512.HashData(TextEncoder.GetBytes(input));
                    break;

                default:
                    return "";
            }

            return EncodingUtil.ToEncodedCryptoString(encodingType, computedHash);
        }
        #endregion
    }
}
