namespace i6tyone_Conference.Models.Dto.Register
{
    public class RegisterInfoRequestDto
    {
        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 3: 얼리 3차 / 4: 일반 / 5: 원데이 / 6: 공식 / 7: 이벤트 / 8: 현장등록)
        /// </summary>
        public List<int> option { get; set; } = new();

        /// <summary>
        /// 신청일 (1: Day1 / 2: Day2 / 3: Day3 / 4: ALL Day)
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
