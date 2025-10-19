using System.ComponentModel.DataAnnotations;

namespace i6tyone_Conference.Models.Dto.Register
{
    public class IssuanceRequestDto
    {
        /// <summary>
        /// 구매자 (필수)
        /// </summary>
        [Required]
        public string buyer { get; set; }

        /// <summary>
        /// 전화번호 (필수)
        /// </summary>
        [Required]
        public string phone { get; set; }
    }
}
