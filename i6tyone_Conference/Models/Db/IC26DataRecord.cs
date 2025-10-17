namespace eGhis_WebService_Core.Models.Db
{
    public record IC26DataRecord
    {
        /// <summary>
        /// 순번
        /// </summary>
        public int IC26No { get; set; }

        /// <summary>
        /// 티켓구분
        /// </summary>
        public string IC26Option { get; set; }

        /// <summary>
        /// 신청일
        /// </summary>
        public string IC26Day { get; set; }

        /// <summary>
        /// 구매자
        /// </summary>
        public string IC26Buyer { get; set; }

        /// <summary>
        /// 참석자
        /// </summary>
        public string IC26Attender { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string IC26Phone { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
        public string IC26Gender { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public string IC26Age { get; set; }

        /// <summary>
        /// 교회
        /// </summary>
        public string IC26Church { get; set; }

        /// <summary>
        /// 거주지역
        /// </summary>
        public string IC26Local { get; set; }

        /// <summary>
        /// 교단
        /// </summary>
        public string IC26Denom { get; set; }

        /// <summary>
        /// 구매수량
        /// </summary>
        public int IC26Count { get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string IC26Area { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        public string IC26Memo { get; set; }

        /// <summary>
        /// QR Code 발급 키
        /// </summary>
        public string IC26UniqueId { get; set; }

        /// <summary>
        /// 출석여부 (0: 미등록, 1: 등록)
        /// </summary>
        public bool IC26Attend { get; set; }

        /// <summary>
        /// QR 생성여부 (0: 미생성, 1: 생성)
        /// </summary>
        public bool IC26CreateQR { get; set; }

        /// <summary>
        /// SMS 전송여부 (0:미전송, 1:전송완료)
        /// </summary>
        public bool IC26SMS { get; set; }
    }
}
