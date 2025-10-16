using System.Text;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Exceptions;
using eGhis_WebService_Core.Infrastructure.Utils.Codec;

namespace eGhis_WebService_Core.Infrastructure.Utils.Crypto.Symmetric.Base
{
    /// <summary>
    /// 암/복호화 관리 추상 클래스
    /// </summary>
    internal abstract class BaseCryptoSymmetric
    {
        #region FIELD AREA ***************************************************************
        private EEncodingTypes _cryptoStringEncodingType = EEncodingTypes.Default;
        private ECipherModes _cipherMode = ECipherModes.CBC;
        private EPaddingModes _paddingMode = EPaddingModes.PKCS7;
        private Encoding _textEncoder = Encoding.GetEncoding("utf-8");
        private int _keySize = 64;
        private int _blockSize = 64;
        private byte[]? _key = null;
        private byte[]? _iv = null;
        #endregion

        #region PROPERTY AREA *************************************************************
        /// <summary>
        /// 암호화 문자열 타입을 가져오거나 설정합니다.
        /// </summary>
        public EEncodingTypes EncodingType
        {
            get
            {
                return _cryptoStringEncodingType;
            }
            set
            {
                _cryptoStringEncodingType = value;
            }
        }

        /// <summary>
        /// 블럭 암호화 모드를 가져오거나 설정합니다.
        /// </summary>
        public ECipherModes CipherMode
        {
            get
            {
                return _cipherMode;
            }
            set
            {
                _cipherMode = value;
            }
        }

        /// <summary>
        /// 블럭 패딩 모드를 가져오거나 설정합니다.
        /// </summary>
        public EPaddingModes PaddingMode
        {
            get
            {
                return _paddingMode;
            }
            set
            {
                _paddingMode = value;
            }
        }

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

        /// <summary>
        /// 암호화 키의 크기를 가져옵니다.
        /// </summary>
        public virtual int KeySize
        {
            get
            {
                return _keySize;
            }
            protected set
            {
                _keySize = value;
            }
        }

        /// <summary>
        /// 암호화 작업의 블럭 크기를 가져옵니다.
        /// </summary>
        public virtual int BlockSize
        {
            get
            {
                return _blockSize;
            }
            protected set
            {
                _blockSize = value;
            }
        }

        /// <summary>
        /// 암호화 키를 가져오거나 설정합니다.
        /// </summary>
        public virtual byte[]? Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (value?.Length * 8 != KeySize)
                {
                    //throw new CryptoException( "암호화 키의 길이가 잘못되었습니다." );
                }

                _key = value;
            }
        }

        /// <summary>
        /// 초기화 벡터를 가져오거나 설정합니다.
        /// </summary>
        public virtual byte[]? IV
        {
            get
            {
                return _iv;
            }
            set
            {
                if (value?.Length * 8 != BlockSize)
                {
                    throw new CryptoException("초기화 벡터의 길이가 잘못되었습니다.");
                }

                _iv = value;
            }
        }
        #endregion

        #region CONSTRUCTOR AREA *************************************************************
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="keySize">비밀 키 크기</param>
        /// <param name="blockSize">암호화 블럭 크기</param>
        protected BaseCryptoSymmetric(int keySize, int blockSize)
        {
            KeySize = keySize;
            BlockSize = blockSize;

            Key = GenerateKey();
            IV = GenerateIV();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="keySize">비밀 키 크기</param>
        /// <param name="blockSize">암호화 블럭 크기</param>
        /// <param name="key">암호화 키</param>
        /// <param name="iv">초기화 벡터</param>
        protected BaseCryptoSymmetric(int keySize, int blockSize, string key, string iv)
        {
            KeySize = keySize;
            BlockSize = blockSize;

            Key = EncodingUtil.FromHex(key);
            IV = EncodingUtil.FromHex(iv);
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="keySize">비밀 키 크기</param>
        /// <param name="blockSize">암호화 블럭 크기</param>
        /// <param name="key">암호화 키</param>
        /// <param name="iv">초기화 벡터</param>
        protected BaseCryptoSymmetric(int keySize, int blockSize, byte[] key, byte[] iv)
        {
            KeySize = keySize;
            BlockSize = blockSize;

            Key = key;
            IV = iv;
        }
        #endregion

        #region ABSTRACT METHOD AREA ********************************************************
        /// <summary>
        /// 신규 비밀 키를 반환합니다.
        /// </summary>
        /// <returns>비밀 키</returns>
        public abstract byte[] GenerateKey();

        /// <summary>
        /// 신규 초기화 벡터를 반환합니다.
        /// </summary>
        /// <returns>초기화 벡터</returns>
        public abstract byte[] GenerateIV();

        /// <summary>
        /// 입력된 평문을 암호문으로 변환합니다.
        /// </summary>
        /// <param name="plainText">평문</param>
        /// <returns>암호문</returns>
        public abstract string AESEncrypt(string plainText);

        /// <summary>
        /// 입력된 암호문을 평문으로 변환합니다.
        /// </summary>
        /// <param name="cipherText">암호문</param>
        /// <returns>평문</returns>
        public abstract string AESDecrypt(string cipherText);
        #endregion
    }
}
