namespace eGhis_WebService_Core.Models.Dto.Auth
{
    public class QRCodeResponseDto
    {
        /// <summary>
        /// QR 생성 건수
        /// </summary>
        public string successMsg { get; set; }

        /// <summary>
        /// QR 생성 소요시간
        /// </summary>
        public int qrGenTime { get; set; }
    }
}
