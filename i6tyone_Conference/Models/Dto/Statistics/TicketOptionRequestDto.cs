namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class TicketOptionRequestDto
    {
        public IList<Summary> list { get; set; } = new List<Summary>();
    }

    public class Summary
    {
        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 / 3: 얼리 2차 / 4: 공식 / 5: 이벤트 / 6: 현장등록)
        /// </summary>
        public string Option { get; set; }

        /// <summary>
        /// Day1 : 2025-01-27 (화)
        /// </summary>
        public int Day1 { get; set; }

        /// <summary>
        /// Day2 : 2025-01-28 (수)
        /// </summary>
        public int Day2 { get; set; }

        /// <summary>
        /// Day3 : 2025-01-29 (목)
        /// </summary>
        public int Day3 { get; set; }

        /// <summary>
        /// 전체참석
        /// </summary>
        public int AllDay { get; set; }

        /// <summary>
        /// 합계
        /// </summary>
        public int Total { get; set; }
    }
}
