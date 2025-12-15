
namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class DateRequestDto
    {
        /// <summary>
        /// Day1 : 2025-01-27 (화)
        /// </summary>
        public int Day1Count { get; set; }

        /// <summary>
        /// Day2 : 2025-01-28 (수)
        /// </summary>
        public int Day2Count { get; set; }

        /// <summary>
        /// Day3 : 2025-01-29 (목)
        /// </summary>
        public int Day3Count { get; set; }

        /// <summary>
        /// ALL Day
        /// </summary>
        public int AllDayCount { get; set; }
    }
}
