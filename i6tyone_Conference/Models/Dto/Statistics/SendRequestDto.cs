namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class SendRequestDto
    {
        /// <summary>
        /// QR 생성 건수
        /// </summary>
        public int QRNotCreated { get; set; }

        /// <summary>
        /// QR 미생성 건수
        /// </summary>
        public int QRCreated { get; set; }

        /// <summary>
        /// SMS 전송 건수
        /// </summary>
        public int SMSNotSent { get; set; }

        /// <summary>
        /// SMS 미전송 건수
        /// </summary>
        public int SMSSent { get; set; }
    }
}
