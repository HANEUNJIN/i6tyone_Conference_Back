namespace i6tyone_Conference.Models.Dto.Register
{
    public class RegisterInfoResponseDto
    {
        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 / 3: 얼리 2차 / 4: 공식 / 5: 이벤트 / 6: 현장구매 / 7: VIP / 8: 새신자)
        /// </summary>
        public int Option { get; set; }

        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 구매자
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        /// 참석자
        /// </summary>
        public string Attender { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 성별 (M: 남성 / F: 여성)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 교회
        /// </summary>
        public string Church { get; set; }

        /// <summary>
        /// 거주지역
        /// </summary>
        public string Local { get; set; }

        /// <summary>
        /// 교단
        /// </summary>
        public string Denom { get; set; }

        /// <summary>
        /// 새신자여부 (Y/N)
        /// </summary>
        public string newBelieverYn { get; set; }

        /// <summary>
        /// 구매수량
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 출석여부 (N: 미등록, Y: 등록)
        /// </summary>
        public string Attend { get; set; }

        /// <summary>
        /// QR 생성여부 (N: 미생성, Y: 생성)
        /// </summary>
        public string CreateQR { get; set; }

        /// <summary>
        /// SMS 전송여부 (N:미전송, Y:전송완료)
        /// </summary>
        public string SMS { get; set; }

        /// <summary>
        /// Notion 링크 발송 (N:미전송, Y:전송완료)
        /// </summary>
        public string NotionSmsYn { get; set; }

        /// <summary>
        /// 구역배정문자 1차 (N:미전송, Y:전송완료)
        /// </summary>
        public string sms01 { get; set; } = "N";

        /// <summary>
        /// 구역배정문자 2차 (N:미전송, Y:전송완료)
        /// </summary>
        public string sms02 { get; set; } = "N";

        /// <summary>
        /// 구역배정문자 3차 (N:미전송, Y:전송완료)
        /// </summary>
        public string sms03 { get; set; } = "N";

        /// <summary>
        /// 구역배정문자 4차 (N:미전송, Y:전송완료)
        /// </summary>
        public string sms04 { get; set; } = "N";
    }
}
