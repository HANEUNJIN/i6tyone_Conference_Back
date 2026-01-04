using Newtonsoft.Json;

namespace i6tyone_Conference.Models.Dto.AligoMsg
{
    public class AligoGenericResponse
    {
        /// <summary>
        /// 결과코드(API 수신유무)
        /// </summary>
        [JsonProperty("result_code")]
        public string resultCode { get; set; }

        /// <summary>
        /// 결과 메시지( result_code 가 0 보다 작은경우 실패사유 표기)
        /// </summary>
        [JsonProperty("message")]
        public string message { get; set; }
    }
}
