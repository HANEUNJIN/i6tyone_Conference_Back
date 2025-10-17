namespace eGhis_WebService_Core.Models.Dto.Auth
{
    public class RegisterRequestDto
    {
        /// <summary>
        /// 티켓구분
        /// </summary>
        public string iC26Option { get; set; }

        /// <summary>
        /// 신청일
        /// </summary>
        public string iC26Day { get; set; }

        /// <summary>
        /// 구매자
        /// </summary>
        public string iC26Buyer { get; set; }

        /// <summary>
        /// 참석자
        /// </summary>
        public string iC26Attender { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string iC26Phone { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
        public string iC26Gender { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public string iC26Age { get; set; }

        /// <summary>
        /// 교회
        /// </summary>
        public string iC26Church { get; set; }

        /// <summary>
        /// 거주지역
        /// </summary>
        public string iC26Local { get; set; }

        /// <summary>
        /// 교단
        /// </summary>
        public string iC26Denom { get; set; }

        /// <summary>
        /// 구매수량
        /// </summary>
        public int iC26Count { get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string iC26Area { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        public string iC26Memo { get; set; }

        /// <summary>
        /// 출석여부 (0: 미등록, 1: 등록)
        /// </summary>
        public bool iC26Attend { get; set; }

        /// <summary>
        /// QR 생성여부 (0: 미생성, 1: 생성)
        /// </summary>
        public bool iC26CreateQR { get; set; }

        /// <summary>
        /// SMS 전송여부 (0:미전송, 1:전송완료)
        /// </summary>
        public bool iC26SMS { get; set; }
    }
}
