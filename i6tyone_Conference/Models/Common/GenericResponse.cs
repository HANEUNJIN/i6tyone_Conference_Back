using Newtonsoft.Json;

namespace eGhis_WebService_Core.Models.Common
{
    public class GenericResponse<T> : ResponseBaseModel where T : class
    {
        [JsonProperty("Data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        public GenericResponse(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            // Data를 초기화하지 않음 → 필요할 때만 수동으로 할당
        }

        public GenericResponse() : base(null)
        {
            // Data를 초기화하지 않음 → 필요할 때만 수동으로 할당
        }
    }
}
