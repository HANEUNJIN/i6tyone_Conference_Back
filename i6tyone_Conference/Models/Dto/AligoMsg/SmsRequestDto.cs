namespace i6tyone_Conference.Models.Dto.AligoMsg
{
    public class SmsRequestDto
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
        /// 메시지 고유ID
        /// </summary>
        public long mid { get; set; }

        /// <summary>
        /// 페이지번호
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 페이지당 출력갯수
        /// </summary>
        public int pageSize { get; set; }
    }
}
