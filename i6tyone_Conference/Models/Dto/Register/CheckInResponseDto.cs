namespace i6tyone_Conference.Models.Dto.Auth
{
    public class CheckInResponseDto
    {
        /// <summary>
        /// 안내 메시지
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 구매자
        /// </summary>
        public string iC26Buyer { get; set; }

        /// <summary>
        /// 티켓구분
        /// </summary>
        public string iC26Option { get; set; }

        /// <summary>
        /// 신청일
        /// </summary>
        public string iC26Day { get; set; }

        /// <summary>
        /// 구매수량
        /// </summary>
        public int iC26Count { get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string iC26Area { get; set; }
    }
}
