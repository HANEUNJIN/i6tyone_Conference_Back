using CsvHelper;
using CsvHelper.Configuration;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Excel;
using System.Globalization;
using System.Text;

namespace i6tyone_Conference.Service.Excel
{
    public class ExcelService : IExcelService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;
        private readonly GoogleUtil _googleUtil;
        private readonly OnSiteGoogleUtil _onSiteGoogleUtil;

        public ExcelService(IDbConnectionFactory connFactory, ISqlRepository repo, GoogleUtil googleUtil, OnSiteGoogleUtil onSiteGoogleUtil)
        {
            _connFactory = connFactory;
            _repo = repo;
            _googleUtil = googleUtil;
            _onSiteGoogleUtil = onSiteGoogleUtil;
        }

        public async Task<GenericResponse<RegisterAddResponseDto>> GetGoogleSheetAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterAddResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            int successCount = 0;

            try
            {
                using var client = new HttpClient();
                string csvData = await client.GetStringAsync(_googleUtil.CsvUrl, cancellationToken);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true, // 헤더 존재
                    BadDataFound = null, // 잘못된 데이터 무시
                    DetectDelimiter = false,
                    Delimiter = ","
                };

                using var reader = new StringReader(csvData);
                using var csv = new CsvReader(reader, config);

                for (int i = 0; i < 2; i++)
                    reader.ReadLine();

                try
                {
                    var records = csv.GetRecords<GoogleCsvRow>().ToList();
                        
                    foreach (var row in records)
                    {
                        string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);

                        var req = new RegisterRequestDto()
                        {
                            area = row.Area,
                            option = ConvertOption(row.Option),
                            day = ConvertDay(row.Day),
                            buyer = row.Buyer,
                            attender = row.Attender,
                            phone = row.Phone?.Replace("-", ""),
                            gender = ConvertGender(row.Gender),
                            age = ToShortOrZero(row.Age),
                            church = row.Church,
                            local = row.Local,
                            denom = row.Denom,
                            count = ToShortOrZero(row.Count),
                            memo = row.Memo,
                            notionSmsYn = row.NotionSmsYn == "O" ? "Y" : "N",
                            newBelieverYn = "",
                            sms02 = row.Sms02 == "O" ? "Y" : "N",
                            sms03 = row.Sms03 == "O" ? "Y" : "N",
                            sms04 = row.Sms04 == "O" ? "Y" : "N"
                        };

                        var data = await _repo.IC26DataDao.GenerateRegisterAsync(db, req, uniqueId);
                        if (data < 0)
                        {
                            res.SetResult(ErrorStatusCode.Invalid_Error);
                            res.ResultMsg = "'2026 Solus CHRISTUS' 연동 중 오류 발생.";
                            return res;
                        }

                        successCount++;
                    }
                }
                catch (Exception)
                {
                    res.SetResult("", "이벤터스 연동 엑셀 컬럼이 올바르지 않습니다.");
                    return res;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Insert_Error, $"[Google Sheet DB INSERT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new RegisterAddResponseDto() { successCount = successCount };
            return res;
        }

        public async Task<GenericResponse<RegisterAddResponseDto>> GetOnSiteGoogleSheetAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterAddResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            int successCount = 0;

            try
            {
                using var client = new HttpClient();
                string csvData = await client.GetStringAsync(_onSiteGoogleUtil.CsvUrl, cancellationToken);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    BadDataFound = null,
                    DetectDelimiter = false,
                    Delimiter = ","
                };

                using var reader = new StringReader(csvData);
                using var csv = new CsvReader(reader, config);

                var records = csv.GetRecords<OnSiteCsvRow>().ToList();

                foreach (var row in records)
                {
                    string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);

                    var req = new RegisterRequestDto()
                    {
                        option = 6,
                        day = ConvertDay(row.참석날짜),
                        buyer = row.성함,
                        attender = row.성함,
                        phone = row.연락처?.Replace("-", ""),
                        gender = ConvertGender(row.성별),
                        age = ToShortOrZero(row.나이),
                        church = row.출석교회,
                        local = row.거주지역,
                        denom = row.교단,
                        count = ToShortOrZero(row.구매수량),
                        applyYmd = ConvertDateTime(row.타임스탬프)
                    };

                    var data = await _repo.IC26DataDao.OnSiteGenerateRegisterAsync(db, req, uniqueId);
                    if (data < 0)
                    {
                        res.SetResult(ErrorStatusCode.Invalid_Error);
                        res.ResultMsg = "'26 conf. 현장 등록(응답)' 연동 중 오류 발생.";
                        return res;
                    }

                    successCount++;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Insert_Error, $"[Google Sheet DB INSERT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new RegisterAddResponseDto() { successCount = successCount };
            return res;
        }

        public async Task<GenericResponse<RegisterAddResponseDto>> GetEventUsSheetAsync(CsvRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterAddResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (req.file == null || req.file.Length == 0)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "업로드된 파일이 없습니다.";
                return res;
            }

            int successCount = 0;

            try
            {
                using var stream = req.file.OpenReadStream();
                using var reader = new StreamReader(stream, Encoding.GetEncoding("euc-kr"));

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    BadDataFound = null,
                    TrimOptions = TrimOptions.Trim,
                    Mode = CsvMode.RFC4180
                };

                using var csv = new CsvReader(reader, config);

                for (int i = 0; i < 3; i++)
                    reader.ReadLine();

                csv.Context.RegisterClassMap<CsvRowMap>();

                var rows = csv.GetRecords<CsvRowDto>().ToList();

                foreach (var row in rows)
                {
                    if (req.updateYmd >= DateTime.Parse(row.신청일시))
                        continue;

                    string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);

                    var rowReq = new RegisterRequestDto()
                    {
                        area = string.Empty,
                        option = req.option,
                        day = req.day,
                        buyer = row.이름,
                        attender = ToShortOrZero(row.수량) == 1 ? row.이름 : string.Empty,
                        phone = row.휴대전화번호,
                        gender = ConvertGender(row.성별),
                        age = ToShortOrZero(row.나이),
                        church = row.출석교회,
                        local = row.지역,
                        denom = row.교단,
                        count = ToShortOrZero(row.수량),
                        memo = string.Empty,
                        notionSmsYn = "N",
                        newBelieverYn = "",
                        applyYmd = DateTime.Parse(row.신청일시).ToString(),
                    };

                    var data = await _repo.IC26DataDao.GenerateRegisterAsync(db, rowReq, uniqueId);
                    if (data < 0)
                    {
                        res.SetResult(ErrorStatusCode.Invalid_Error);
                        res.ResultMsg = $"CSV 데이터 등록 실패";
                        return res;
                    }

                    successCount++;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Insert_Error, $"[CSV DB INSERT ERROR] Failed to process CSV. Reason: {ex.Message}");

                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "CSV 처리 중 오류가 발생했습니다.";
                return res;
            }

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new RegisterAddResponseDto { successCount = successCount };
            return res;
        }

        private short ToShortOrZero(string value)
        {
            return short.TryParse(value, out var result) ? result : (short)0;
        }

        private int ConvertOption(string option)
        {
            switch (option)
            {
                case "슈퍼얼리":
                    return 1;
                case "얼리1차":
                    return 2;
                case "얼리2차":
                    return 3;
                case "공식":
                    return 4;
                case "이벤트":
                    return 5;
                case "현장구매":
                    return 6;
                case "VIP":
                    return 7;
            }
            return 0;
        }

        private int ConvertDay(string day)
        {
            switch (day)
            {
                case "[화]":
                case "1/27 (화)":
                    return 1;
                case "[수]":
                case "1/28 (수)":
                    return 2;
                case "[목]":
                case "1/29 (목)":
                    return 3;
                case "3-day":
                    return 4;
            }
            return 0;
        }

        private string ConvertGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender))
                return string.Empty;

            if (new[] { "남성", "남자", "남" }.Contains(gender))
                return "M";

            if (new[] { "여성", "여자", "여" }.Contains(gender))
                return "F";

            if (new[] { "남여", "남녀" }.Contains(gender))
                return "M/F";

            return string.Empty;
        }

        private string ConvertDateTime(string dateTime)
        {
            if (DateTime.TryParse(dateTime, new CultureInfo("ko-KR"), DateTimeStyles.None, out DateTime dt))
                return dt.ToString("yyyy-MM-dd HH:mm:ss");

            return string.Empty;
        }
    }

    public class CsvRowDto
    {
        public string 신청일시 { get; set; }
        public string 이름 { get; set; }
        public string 성별 { get; set; }
        public string 휴대전화번호 { get; set; }
        public string 출석교회 { get; set; }
        public string 지역 { get; set; }
        public string 교단 { get; set; }
        public string 수량 { get; set; }
        public string 나이 { get; set; }
    }

    public sealed class CsvRowMap : ClassMap<CsvRowDto>
    {
        public CsvRowMap()
        {
            Map(m => m.신청일시).Name("신청일시");
            Map(m => m.이름).Name("이름", "\"이름\"");
            Map(m => m.성별).Name("성별");
            Map(m => m.휴대전화번호).Name("휴대전화번호");
            Map(m => m.출석교회).Name("출석교회");
            Map(m => m.지역).Name("지역");
            Map(m => m.교단).Name("교단");
            Map(m => m.수량).Name("수량");
            Map(m => m.나이).Name("나이");
        }
    }

    public class OnSiteCsvRow
    {
        public string 타임스탬프 { get; set; }
        public string 참석날짜 { get; set; }
        public string 성함 { get; set; }
        public string 연락처 { get; set; }
        public string 성별 { get; set; }
        public string 나이 { get; set; }
        public string 출석교회 { get; set; }
        public string 거주지역 { get; set; }
        public string 교단 { get; set; }
        public string 구매수량 { get; set; }
    }
}
