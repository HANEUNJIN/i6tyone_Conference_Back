namespace i6tyone_Conference.Models.Dto.Register
{
    public class RegisterInfoRequestDto
    {
        /// <summary>
        /// 신청일 (1: Day1 / 2: Day2 / 3: Day3 / 4: ALL)
        /// </summary>
        public int day { get; set; }

        /// <summary>
        /// 구매자
        /// </summary>
        public string buyer {  get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 교회
        /// </summary>
        public string church { get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public string area { get; set; }
    }
}
