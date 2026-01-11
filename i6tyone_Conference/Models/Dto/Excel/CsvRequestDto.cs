using System.ComponentModel.DataAnnotations;

namespace i6tyone_Conference.Models.Dto.Excel
{
    public class CsvRequestDto
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
        /// 업데이트 날짜
        /// </summary>
        [Required]
        public DateTime updateYmd { get; set; }

        /// <summary>
        /// .CSV 파일
        /// </summary>
        [Required]
        public IFormFile file { get; set; }
    }
}
