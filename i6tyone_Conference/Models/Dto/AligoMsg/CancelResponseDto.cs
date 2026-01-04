using Newtonsoft.Json;

namespace i6tyone_Conference.Models.Dto.Aligo
{
    public class CancelResponseDto
    {
        /// <summary>
        /// 취소일자(YYYY-MM-DD HH:II:SS)
        /// </summary>
        [JsonProperty("cancel_date")]
        public string cancelDate { get; set; }
    }
}
