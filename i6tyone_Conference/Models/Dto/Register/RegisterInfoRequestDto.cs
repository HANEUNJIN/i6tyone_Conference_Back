namespace i6tyone_Conference.Models.Dto.Register
{
    public class RegisterInfoRequestDto
    {
        /// <summary>
        /// 신청일 (0: 전체 / 1: Day1 / 2: Day2 / 3: Day3 / 4: ALL Day)
        /// </summary>
        public int day { get; set; }

        /// <summary>
        /// 구매자, 참석자, 전화번호, 교회, 교단
        /// </summary>
        public string keyword {  get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 현재 페이지
        /// </summary>
        public int pageNum { get; set; } = 1;

        /// <summary>
        /// 한 페이지 크기
        /// </summary>
        public int pageSize { get; set; } = 20;
    }
}
