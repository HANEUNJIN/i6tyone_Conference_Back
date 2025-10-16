namespace eGhis_WebService_Core.Models.Config
{
    public class JwtSettings
    {
        /// <summary>
        /// JWT 서명용 시크릿 키 (HMAC SHA256)
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 액세스 토큰 유효시간 (초)
        /// </summary>
        public int AccessTokenExpireSeconds { get; set; }

        /// <summary>
        /// 리프레시 토큰 유효시간 (초)
        /// </summary>
        public int RefreshTokenExpireSeconds { get; set; }

        /// <summary>
        /// JWT 발급자 (옵션)
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// JWT 대상자 (옵션)
        /// </summary>
        public string Audience { get; set; }
    }
}
