using Newtonsoft.Json;

namespace i6tyone_Conference.Models.Dto.Aligo
{
    public class SmsResponseDto
    {
        /// <summary>
        /// 목록 배열
        /// </summary>
        [JsonProperty("list")]
        public List<SmsInfo> list { get; set; }

        /// <summary>
        /// 다음조회목록 유무
        /// </summary>
        [JsonProperty("next_yn")]
        public string nextYn { get; set; }
    }

    public class SmsInfo
    {
        /// <summary>
        /// 메시지 상세 ID
        /// </summary>
        [JsonProperty("mdid")]
        public long mdid { get; set; }

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
        /// 수신번호
        /// </summary>
        [JsonProperty("receiver")]
        public string receiver { get; set; }

        /// <summary>
        /// 전송상태
        /// </summary>
        [JsonProperty("sms_state")]
        public string smsState { get; set; }

        /// <summary>
        /// 등록일(YYYY-MM-DD HH:ii:ss)
        /// </summary>
        [JsonProperty("reg_date")]
        public string regDate { get; set; }

        /// <summary>
        /// 전송일(YYYY-MM-DD HH:ii:ss)
        /// </summary>
        [JsonProperty("send_date")]
        public string sendDate { get; set; }

        /// <summary>
        /// 예약일(YYYY-MM-DD HH:ii:ss)
        /// </summary>
        [JsonProperty("reserve_date")]
        public string reserveDate { get; set; }
    }
}
