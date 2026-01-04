namespace i6tyone_Conference.Models.Dto.AligoMsg
{
    public class SendRequestDto
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
        /// 페이지번호
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 페이지당 출력갯수
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 조회시작일자
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 조회마감일자
        /// </summary>
        public string limitDay { get; set; }
    }
}
