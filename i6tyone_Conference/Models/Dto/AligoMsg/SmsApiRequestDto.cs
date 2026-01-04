using System.ComponentModel.DataAnnotations;

namespace i6tyone_Conference.Models.Dto.Aligo
{
    public class SmsApiRequestDto
    {
        /// <summary>
        /// 메시지 고유ID
        /// </summary>
        [Required]
        public long mid { get; set; }

        /// <summary>
        /// 페이지번호
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 페이지당 출력갯수
        /// </summary>
        public int pageSize { get; set; }
    }
}
