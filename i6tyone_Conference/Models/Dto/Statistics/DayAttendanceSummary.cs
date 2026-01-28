namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class DayAttendanceSummary
    {
        public IList<DayAttendance> list { get; set; } = new List<DayAttendance>();
    }

    public class DayAttendance
    {
        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public string day { get; set; }

        /// <summary>
        /// 티켓구분 (1: 슈퍼얼리 / 2: 얼리 1차 / 3: 얼리 2차 / 4: 공식 / 5: 이벤트 / 6: 현장구매 / 7: VIP / 8: 새신자)
        /// </summary>
        public string option { get; set; }

        /// <summary>
        /// 총 등록자 수
        /// </summary>
        public int totalCount { get; set; }

        /// <summary>
        /// 신청일별 총 등록자 수
        /// </summary>
        public int dayTotalCount { get; set; }
    }
}
