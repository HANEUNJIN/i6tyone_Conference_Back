namespace eGhis_WebService_Core.Models.Common
{
    public class JwtPayloadModel
    {
        /// <summary>
        /// 고유키
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 이름
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 연락처
        /// </summary>
        public string phoneNumber { get; set; }

        /// <summary>
        /// 토큰 발급 시간 (Unix Timestamp)
        /// </summary>
        public long iat { get; set; }

        /// <summary>
        /// 토큰 만료 시간 (Unix Timestamp)
        /// </summary>
        public long exp { get; set; }
    }
}
