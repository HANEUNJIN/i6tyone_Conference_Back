using static MongoDB.Driver.WriteConcern;

namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class DateRequestDto
    {
        /// <summary>
        /// 01.27 TUE
        /// </summary>
        public int Day1Count { get; set; }

        /// <summary>
        /// 01.28 WED
        /// </summary>
        public int Day2Count { get; set; }

        /// <summary>
        /// 01.29 THU
        /// </summary>
        public int Day3Count { get; set; }
    }
}
