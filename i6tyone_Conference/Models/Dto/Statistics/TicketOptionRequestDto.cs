namespace i6tyone_Conference.Models.Dto.Statistics
{
    public class TicketOptionRequestDto
    {
        public IList<Summary> list { get; set; } = new List<Summary>();
    }

    public class Summary
    {
        /// <summary>
        /// 신청일 (1: 화 / 2: 수 / 3: 목 / 4: 3-day)
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// 슈퍼얼리
        /// </summary>
        public int SuperEarly { get; set; }

        /// <summary>
        /// 얼리 1차
        /// </summary>
        public int Early1 { get; set; }

        /// <summary>
        /// 얼리 2차
        /// </summary>
        public int Early2 { get; set; }

        /// <summary>
        /// 얼리 합계
        /// </summary>
        public int EarlyTotal { get; set; }

        /// <summary>
        /// 공식
        /// </summary>
        public int Regular { get; set; }

        /// <summary>
        /// 이벤트
        /// </summary>
        public int Event { get; set; }

        /// <summary>
        /// 현장구매
        /// </summary>
        public int Site { get; set; }


        /// <summary>
        /// VIP
        /// </summary>
        public int Vip { get; set; }

        /// <summary>
        /// 새신자
        /// </summary>
        public int NewBeliever { get; set; }

        /// <summary>
        /// 합계
        /// </summary>
        public int Total { get; set; }
    }
}
