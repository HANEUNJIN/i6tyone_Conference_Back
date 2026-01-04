namespace i6tyone_Conference.Models.Dto.AligoMsg
{
    public class CancelRequestDto
    {
        /// <summary>
        /// 인증용 API Key
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 메시지 ID
        /// </summary>
        public long mid { get; set; }
    }
}
