namespace i6tyone_Conference.Models.Dto.Register
{
    public class RegisterUpdateRequestDto
    {
        /// <summary>
        /// QR Code 발급 키
        /// </summary>
        public string uniqueIdKey { get; set; }

        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 3: 얼리 3차 / 4: 일반 / 5: 원데이 / 6: 공식 / 7: 이벤트 / 8: 현장등록)
        /// </summary>
        public int option { get; set; }

        /// <summary>
        /// 신청일 (1: Day1 / 2: Day2 / 3: Day3 / 4: ALL Day)
        /// </summary>
        public int day { get; set; }

        /// <summary>
        /// 구매자
        /// </summary>
        public string buyer { get; set; }

        /// <summary>
        /// 참석자
        /// </summary>
        public string attender { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 성별 (M: 남성 / F: 여성)
        /// </summary>
        public string gender { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public int age { get; set; }

        /// <summary>
        /// 교회
        /// </summary>
        public string church { get; set; }

        /// <summary>
        /// 거주지역
        /// </summary>
        public string local { get; set; }

        /// <summary>
        /// 교단
        /// </summary>
        public string denom { get; set; }

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

        /// <summary>
        /// 메모
        /// </summary>
        public string memo { get; set; }
    }
}
