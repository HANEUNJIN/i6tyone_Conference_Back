namespace i6tyone_Conference.Models.Dto.Register
{
    public class RegisterInfoRequestDto
    {
        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 / 3: 얼리 2차 / 4: 공식 / 5: 이벤트 / 6: 현장구매 / 7: VIP)
        /// </summary>
        public List<int> option { get; set; } = new();

        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public List<int> day { get; set; } = new();

        /// <summary>
        /// 구매자, 참석자, 전화번호, 교회, 교단
        /// </summary>
        public string keyword {  get; set; }

        /// <summary>
        /// 좌석구역
        /// </summary>
        public List<string> area { get; set; } = new();

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
