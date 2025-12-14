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
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 3: 얼리 3차 / 4: 일반 / 5: 원데이 / 6: 공식 / 7: 이벤트 / 8: 현장등록)
        /// </summary>
        public int option { get; set; }

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
