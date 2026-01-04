using Newtonsoft.Json;

namespace i6tyone_Conference.Models.Dto.Aligo
{
    public class SendMassResponseDto
    {
        /// <summary>
        /// 메시지 고유ID
        /// </summary>
        [JsonProperty("msg_id")]
        public string msgId { get; set; }

        /// <summary>
        /// 요청성공 건수
        /// </summary>
        [JsonProperty("success_cnt")]
        public int successCnt { get; set; }

        /// <summary>
        /// 요청실패 건수
        /// </summary>
        [JsonProperty("error_cnt")]
        public int errorCnt { get; set; }

        /// <summary>
        /// 메시지 타입 (1. SMS, 2.LMS, 3. MMS)
        /// </summary>
        [JsonProperty("msg_type")]
        public string msgType { get; set; }
    }
}
