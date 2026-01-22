using CsvHelper.Configuration.Attributes;

namespace i6tyone_Conference.Models.Dto.Excel
{
    public class GoogleCsvRow
    {
        [Name("구역")]
        public string Area { get; set; }

        [Name("구분")]
        public string Option { get; set; }

        [Name("참석일")]
        public string Day { get; set; }

        [Name("구매자")]
        public string Buyer { get; set; }

        [Name("참석자")]
        public string Attender { get; set; }

        [Name("연락처")]
        public string Phone { get; set; }

        [Name("성별")]
        public string Gender { get; set; }

        [Name("나이")]
        public string Age { get; set; }

        [Name("출석교회")]
        public string Church { get; set; }

        [Name("지역")]
        public string Local { get; set; }

        [Name("교단")]
        public string Denom { get; set; }

        [Name("수량")]
        public string Count { get; set; }

        [Name("비고")]
        public string Memo { get; set; }

        [Name("노션링크 발송")]
        public string NotionSmsYn { get; set; }

        [Name("구역문자 2차")]
        public string Sms02 { get; set; }

        [Name("구역문자 3차")]
        public string Sms03 { get; set; }

        [Name("구역문자 4차")]
        public string Sms04 { get; set; }
    }
}
