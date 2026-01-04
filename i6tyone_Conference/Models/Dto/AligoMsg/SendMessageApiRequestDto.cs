using System.ComponentModel.DataAnnotations;

namespace i6tyone_Conference.Models.Dto.Aligo
{
    public class SendMessageApiRequestDto
    {
        /// <summary>
        /// 수신자 전화번호 - 컴마(,)분기 입력으로 최대 1천명
        /// </summary>
        [Required]
        public string receiver { get; set; }

        /// <summary>
        /// 메시지 내용
        /// </summary>
        [Required]
        public string msg { get; set; }

        /// <summary>
        /// SMS(단문) , LMS(장문), MMS(그림문자) 구분
        /// </summary>
        public string msgType { get; set; }

        /// <summary>
        /// 문자제목(LMS,MMS만 허용)
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// %고객명% 치환용 입력
        /// </summary>
        public string destination { get; set; }

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
        public byte[] image1 { get; set; }

        /// <summary>
        /// 첨부이미지
        /// </summary>
        public byte[] image2 { get; set; }

        /// <summary>
        /// 첨부이미지
        /// </summary>
        public byte[] image3 { get; set; }

        /// <summary>
        /// 연동테스트시 Y 적용
        /// </summary>
        public string testModeYn { get; set; }
    }
}
