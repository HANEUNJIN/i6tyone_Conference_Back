namespace eGhis_WebService_Core.Models.Db
{
    public record IC26DataRecord
    {
        /// <summary>
        /// 순번
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 3: 얼리 3차 / 4: 일반 / 5: 원데이)
        /// </summary>
        public int Option { get; set; }

        /// <summary>
        /// 신청일 (1: Day1 / 2: Day2 / 3: Day3 / 4: ALL)
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
        /// QR Code 발급 키
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        /// 출석여부 (0: 미등록, 1: 등록)
        /// </summary>
        public bool Attend { get; set; }

        /// <summary>
        /// QR 생성여부 (0: 미생성, 1: 생성)
        /// </summary>
        public bool CreateQR { get; set; }

        /// <summary>
        /// SMS 전송여부 (0:미전송, 1:전송완료)
        /// </summary>
        public bool SMS { get; set; }
    }
}
