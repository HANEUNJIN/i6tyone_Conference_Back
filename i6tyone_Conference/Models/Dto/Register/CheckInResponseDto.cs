namespace i6tyone_Conference.Models.Dto.Auth
{
    public class CheckInResponseDto
    {
        /// <summary>
        /// 구매자
        /// </summary>
        public string buyer { get; set; }

        /// <summary>
        /// 참석자
        /// </summary>
        public string attender { get; set; }

        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 / 3: 얼리 2차 / 4: 공식 / 5: 이벤트 / 6: 현장구매 / 7: VIP)
        /// </summary>
        public int option { get; set; }

        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public int day { get; set; }

        /// <summary>
        /// 교회
        /// </summary>
        public string church { get; set; }

        /// <summary>
        /// 구매수량
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 출석여부 (N: 미등록, Y: 등록)
        /// </summary>
        public string attend { get; set; }
    }
}
