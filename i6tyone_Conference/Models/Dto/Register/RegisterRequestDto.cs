using System.ComponentModel.DataAnnotations;

namespace eGhis_WebService_Core.Models.Dto.Auth
{
    public class RegisterRequestDto
    {
        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 / 3: 얼리 2차 / 4: 공식 / 5: 이벤트 / 6: 현장구매 / 7: VIP)
        /// </summary>
        [Required]
        public int option { get; set; }

        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        [Required]
        public int day { get; set; }

        /// <summary>
        /// 구매자
        /// </summary>
        [Required]
        public string buyer { get; set; }

        /// <summary>
        /// 참석자
        /// </summary>
        public string attender { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        [Required]
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
        /// 새신자여부 (Y/N)
        /// </summary>
        public string newBelieverYn { get; set; }

        /// <summary>
        /// 구매수량
        /// </summary>
        [Required]
        public int count { get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        public string memo { get; set; }

        /// <summary>
        /// Notion 링크 발송 (N:미전송, Y:전송완료)
        /// </summary>
        public string notionSmsYn { get; set; }

        /// <summary>
        /// 등록일자
        /// </summary>
        public string applyYmd { get; set; }
    }
}
