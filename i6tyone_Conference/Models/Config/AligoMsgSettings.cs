namespace i6tyone_Conference.Models.Config
{
    public class AligoMsgSettings
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 인증용 API Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 발신자 전화번호 (최대 16bytes)
        /// </summary>
        public string Sender { get; set; }
    }
}
