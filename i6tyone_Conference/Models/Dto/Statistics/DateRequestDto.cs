namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class DateRequestDto
    {
        public IList<DateInfo> list { get; set; } = new List<DateInfo>();
    }

    public class DateInfo
    {
        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// 전체 데이터 수
        /// </summary>
        public int Total { get; set; }
    }
}
