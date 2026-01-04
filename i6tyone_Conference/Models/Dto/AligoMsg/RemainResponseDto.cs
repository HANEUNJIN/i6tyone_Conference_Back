using Newtonsoft.Json;

namespace i6tyone_Conference.Models.Dto.AligoMsg
{
    public class RemainResponseDto
    {
        /// <summary>
        /// 단문전송시 발송가능한건수
        /// </summary>
        [JsonProperty("SMS_CNT")]
        public int smsCnt { get; set; }

        /// <summary>
        /// 단문전송시 발송가능한건수
        /// </summary>
        [JsonProperty("LMS_CNT")]
        public int lmsCnt { get; set; }

        /// <summary>
        /// 그림(사진)전송시 발송가능한건수
        /// </summary>
        [JsonProperty("MMS_CNT")]
        public int mmsCnt { get; set; }
    }
}
