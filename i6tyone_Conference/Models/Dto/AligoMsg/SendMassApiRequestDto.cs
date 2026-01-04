using System.ComponentModel.DataAnnotations;

namespace i6tyone_Conference.Models.Dto.Aligo
{
    public class SendMassApiRequestDto
    {
        /// <summary>
        /// 수신자 전화번호1
        /// </summary>
        [Required]
        public string rec1 { get; set; }

        /// <summary>
        /// 메시지 내용1
        /// </summary>
        [Required]
        public string msg1 { get; set; }

        /// <summary>
        /// 메시지 전송건수(번호,메시지 매칭건수)
        /// </summary>
        [Required]
        public int cnt { get; set; }

        /// <summary>
        /// 문자제목(LMS,MMS만 허용)
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// SMS(단문) , LMS(장문), MMS(그림문자) 구분
        /// </summary>
        [Required]
        public string msgType { get; set; }

        /// <summary>
        /// 예약일 (현재일이상)
        /// </summary>
        public string rDate { get; set; }

        /// <summary>
        /// 예약시간 - 현재시간기준 10분이후
        /// </summary>
        public string rTime { get; set; }

        /// <summary>
        /// 첨부이미지 (image 또는 image1)
        /// </summary>
        public string image1 { get; set; }

        /// <summary>
        /// 첨부이미지
        /// </summary>
        public string image2 { get; set; }

        /// <summary>
        /// 첨부이미지
        /// </summary>
        public string image3 { get; set; }

        /// <summary>
        /// 연동테스트시 Y 적용
        /// </summary>
        public string testModeYn { get; set; }
    }
}
