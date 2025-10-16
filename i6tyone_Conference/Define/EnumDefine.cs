namespace eGhis_WebService_Core.Define
{
    #region LOG ********************************************
    public enum LoggerLevel
    {
        ALL,
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }
    #endregion

    #region CIPHER *******************************************
    /// <summary>
	/// 암호화에 사용할 블록 암호화 모드를 지정합니다.
	/// </summary>
	public enum ECipherModes
    {
        /// <summary>
        /// CBC(Cipher Block Chaining) 모드를 통해 피드백이 형성됩니다.일반 텍스트 블록이 암호화되기 전에 비트 배타적 OR
        /// 연산에 의해 이전 블록의 암호화 텍스트와 결합됩니다.따라서 일반 텍스트에 여러 개의 동일한 블록이 들어 있어도 각각 다른 암호화 텍스트
        /// 블록으로 암호화됩니다.블록이 암호화되기 전에 비트 배타적 OR 연산에 의해 초기화 벡터에 첫 번째 일반 텍스트 블록이 결합됩니다.암호화
        /// 텍스트 블록의 한 비트라도 손상되면 대응하는 일반 텍스트 블록도 손상됩니다.또한 손상된 원본 비트와 같은 위치에 있는 다음 블록의
        /// 비트도 손상됩니다.
        /// </summary>
        CBC = 1,

        /// <summary>
        /// ECB(Electronic Codebook) 모드는 각 블록을 개별적으로 암호화합니다.이것은 동일하면서 같은 메시지에 포함되어 있는
        /// 일반 텍스트의 블록이나 같은 키로 암호화된 다른 메시지에 있는 일반 텍스트의 블록이 모두 동일한 암호화 텍스트 블록으로 변환됨을 의미합니다.암호화할
        /// 일반 텍스트에 반복 부분이 포함되어 있으면 암호화 텍스트를 한 번에 하나의 블록으로 쉽게 구분할 수 있습니다.또한 다른 사용자가 몰래
        /// 개별 블록을 대체하거나 교환할 수 있습니다.암호화 텍스트 블록의 한 비트라도 손상되면 대응하는 일반 텍스트 블록 전체가 손상됩니다.
        /// </summary>
        ECB = 2,

        /// <summary>
        /// OFB(Output Feedback) 모드는 한 번에 전체 블록을 처리하지 않고 일반 텍스트를 조금씩 암호화 텍스트로 처리합니다.이
        /// 모드는 CFB와 비슷하며 이동 레지스터를 채우는 방식에서만 차이가 있습니다.암호화 텍스트의 한 비트가 손상되면 일반 텍스트의 해당
        /// 비트도 손상됩니다.그러나 암호화 텍스트에 추가 비트나 누락된 비트가 있으면 그 지점부터 일반 텍스트가 손상됩니다.
        /// </summary>
        OFB = 3,

        /// <summary>
        /// CFB(Cipher Feedback) 모드는 한 번에 전체 블록을 처리하지 않고 일반 텍스트를 조금씩 암호화 텍스트로 처리합니다.이
        /// 모드는 한 블록 길이를 가지며 섹션으로 구분되는 이동 레지스터를 사용합니다.예를 들어, 블록 크기가 한 번에 1바이트씩 처리되는 8바이트이면
        /// 이동 레지스터는 여덟 개의 섹션으로 구분됩니다.암호화 텍스트에서 하나의 비트가 손상되면, 하나의 일반 텍스트 비트가 손상되고 이동
        /// 레지스터도 손상됩니다.이로 인해 잘못된 비트가 이동 레지스터 밖으로 이동될 때까지 이어지는 일반 텍스트 비트는 손상됩니다.
        /// </summary>
        CFB = 4,

        /// <summary>
        /// CTS(Cipher Text Stealing) 모드는 길이 제한 없이 일반 텍스트를 처리하고 일반 텍스트와 동일한 길이를 갖는 암호화
        /// 텍스트를 생성합니다.이 모드는 일반 텍스트의 마지막 두 블록을 제외한 모든 블록에 대해 CBC 모드와 동일하게 작동됩니다.
        /// </summary>
        CTS = 5,

        /// <summary>
        /// 블럭 암호화 모드 기본 값
        /// </summary>
        DEFAULT = CBC
    }

    /// <summary>
    /// 메시지 데이터 블록이 암호화 작업에 필요한 전체 바이트 수보다 짧을 때 적용할 패딩 형식을 지정합니다.
    /// </summary>
    public enum EPaddingModes
    {
        /// <summary>
        /// 아무 것도 채워지지 않았습니다.
        /// </summary>
        None = 1,

        /// <summary>
        /// PKCS #7 패딩 문자열은 바이트 시퀀스로 구성되어 있으며, 각각의 시퀀스는 추가된 패딩 바이트의 전체 수와 동일합니다.
        /// </summary>
        PKCS7 = 2,

        /// <summary>
        /// 패딩 문자열은 0으로 설정된 바이트로 구성됩니다.
        /// </summary>
        Zeros = 3,

        /// <summary>
        /// ANSIX923 패딩 문자열에서는 마지막 바이트를 총 패딩 바이트 수로 설정하고 나머지 바이트는 0으로 채웁니다.
        /// </summary>
        ANSIX923 = 4,

        /// <summary>
        /// ISO10126 패딩 문자열에서는 마지막 바이트를 총 패딩 바이트 수로 설정하고 나머지 바이트는 임의의 데이터로 채웁니다.
        /// </summary>
        ISO10126 = 5,

        /// <summary>
        /// 패딩 형식 기본값
        /// </summary>
        DEFAULT = PKCS7
    }

    /// <summary>
    /// 암호화 문자열의 인코딩 타입을 나타냅니다.
    /// </summary>
    public enum EEncodingTypes
    {
        /// <summary>
        /// Base64 String
        /// </summary>
        BASE64 = 0,

        /// <summary>
        /// Hex String
        /// </summary>
        HEX = 1,

        /// <summary>
        /// 암호화 문자열의 인코딩 타입 기본값
        /// </summary>
        Default = BASE64
    }

    /// <summary>
	/// 해쉬 알고리즘 타입
	/// </summary>
	public enum EHashTypes
    {
        /// <summary>
        /// MD5 해쉬
        /// </summary>
        MD5 = 0,

        /// <summary>
        /// SHA256 해쉬
        /// </summary>
        SHA256,

        /// <summary>
        /// SHA384 해쉬
        /// </summary>
        SHA384,

        /// <summary>
        /// SHA512 해쉬
        /// </summary>
        SHA512
    }
    #endregion
}