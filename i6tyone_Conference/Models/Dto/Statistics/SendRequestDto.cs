namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class SendRequestDto
    {
        public IList<SendInfo> list { get; set; } = new List<SendInfo>();
    }

    public class SendInfo
    {
        /// <summary>
        /// QR 미생성 건수
        /// </summary>
        public string YN { get; set; }

        /// <summary>
        /// 출석 건수
        /// </summary>
        public int Attend { get; set; }

        /// <summary>
        /// QR 전송 건수
        /// </summary>
        public int QRSms { get; set; }

        /// <summary>
        /// Notion 전송 건수
        /// </summary>
        public int NotionSms { get; set; }
    }
}
