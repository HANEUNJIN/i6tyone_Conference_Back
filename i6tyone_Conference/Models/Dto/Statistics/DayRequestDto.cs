namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class DayRequestDto
    {
        public IList<DayInfo> list { get; set; } = new List<DayInfo>();
    }

    public class DayInfo
    {
        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// 날짜
        /// </summary>
        public string DateYmd { get; set; }

        /// <summary>
        /// 총 등록자 수
        /// </summary>
        public string TotalCount { get; set; }
    }
}
