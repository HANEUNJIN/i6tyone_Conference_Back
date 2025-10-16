namespace eGhis_WebService_Core.Models.Common
{
    public class SendWebRequestResult
    {
        /// <summary>
        /// http 상태 코드
        /// </summary>
        public int statusCode { get; set; }

        /// <summary>
        /// 결과
        /// </summary>
        public string responseData { get; set; }

        /// <summary>
        /// header 응답시간
        /// </summary>
        public string headerDate { get; set; }
    }
}
