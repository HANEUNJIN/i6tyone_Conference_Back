namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class AreaSeatStatsRequestDto
    {
        public IList<AreaSeatStatsInfo> list { get; set; } = new List<AreaSeatStatsInfo>();
    }

    public class AreaSeatStatsInfo
    {
        /// <summary>
        /// 층 수
        /// </summary>
        public string Floor {  get; set; }

        /// <summary>
        /// 구역
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// 구역
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 이용가능한 좌석 수
        /// </summary>
        public string AvailableCount { get; set; }

        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// 원데이, 올데이
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 원데이 + 올데이
        /// </summary>
        public string TotalCount { get; set; }

        /// <summary>
        /// 남은 좌석 수
        /// </summary>
        public string Remain { get; set; }
    }
}
