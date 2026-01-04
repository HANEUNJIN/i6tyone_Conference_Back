using Newtonsoft.Json;

namespace i6tyone_Conference.Models.Dto.Aligo
{
    public class SendResponseDto
    {
        /// <summary>
        /// 목록 배열
        /// </summary>
        [JsonProperty("list")]
        public List<SendInfo> list { get; set; }

        /// <summary>
        /// 다음조회목록 유무
        /// </summary>
        [JsonProperty("next_yn")]
        public string nextYn { get; set; }
    }

    public class SendInfo
    {
        /// <summary>
        /// 메시지ID
        /// </summary>
        [JsonProperty("mid")]
        public long mid { get; set; }

        /// <summary>
        /// 문자구분(유형)
        /// </summary>
        [JsonProperty("type")]
        public string type { get; set; }

        /// <summary>
        /// 발신번호
        /// </summary>
        [JsonProperty("sender")]
        public string sender { get; set; }

        /// <summary>
        /// 전송요청수
        /// </summary>
        [JsonProperty("sms_count")]
        public int smsCount { get; set; }

        /// <summary>
        /// 요청상태
        /// </summary>
        [JsonProperty("reserve_state")]
        public string reserceState { get; set; }

        /// <summary>
        /// 메시지 내용
        /// </summary>
        [JsonProperty("msg")]
        public string msg { get; set; }

        /// <summary>
        /// 처리실패건수
        /// </summary>
        [JsonProperty("fail_count")]
        public int failCount { get; set; }

        /// <summary>
        /// 등록일(YYYY-MM-DD HH:ii:ss)
        /// </summary>
        [JsonProperty("reg_date")]
        public string regDate { get; set; }

        /// <summary>
        /// 예약일자(YYYY-MM-DD HH:ii:ss)
        /// </summary>
        [JsonProperty("reserve")]
        public string reserve { get; set; }
    }
}
