using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Utils;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace eGhis_WebService_Core.Models.Common
{
    public class ResponseBaseModel
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 응답 데이터
        /// </summary>
        [XmlElement(Order = 1)]
        [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        /// <summary>
        /// 결과 코드
        /// </summary>
        [XmlElement(Order = 9998)]
        [JsonProperty(Order = 9998)]
        public string ResultCd { get; set; }

        /// <summary>
        /// 결과 메시지
        /// </summary>
        [XmlElement(Order = 9999)]
        [JsonProperty(Order = 9999)]
        public string ResultMsg { get; set; }

        public ResponseBaseModel(IHttpContextAccessor httpContextAccessor = null)
        {
            _httpContextAccessor = httpContextAccessor;
            SetResult(ErrorStatusCode.Error); // 기본값은 기타 오류
        }

        /// <summary>
        /// 결과 설정 (enum 기반)
        /// </summary>
        public void SetResult(ErrorStatusCode code, string detail = "")
        {
            ResultCd = EnumUtil.GetDisplayName(code);
            ResultMsg = EnumUtil.GetDescription(code);

            if (!string.IsNullOrEmpty(detail))
            {
                ResultMsg += $" ({detail})";
            }
        }

        /// <summary>
        /// 결과 설정 (직접 코드 입력)
        /// </summary>
        public void SetResult(string code, string message)
        {
            ResultCd = code;
            ResultMsg = message;
        }

        /// <summary>성공 응답</summary>
        public static ResponseBaseModel Success(object data = null)
        {
            var r = new ResponseBaseModel();
            r.SetResult(ErrorStatusCode.Success);
            r.Data = data;
            return r;
        }

        /// <summary>실패 응답 (enum 코드 사용)</summary>
        public static ResponseBaseModel Fail(ErrorStatusCode code, string detail = "", object data = null)
        {
            var r = new ResponseBaseModel();
            r.SetResult(code, detail);
            r.Data = data;
            return r;
        }

        /// <summary>실패 응답 (직접 코드/메시지)</summary>
        public static ResponseBaseModel Fail(string code, string message, object data = null)
        {
            var r = new ResponseBaseModel();
            r.SetResult(code, message);
            r.Data = data;
            return r;
        }
    }
}
