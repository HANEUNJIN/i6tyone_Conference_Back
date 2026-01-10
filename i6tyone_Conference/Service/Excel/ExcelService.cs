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

        public ExcelService(IDbConnectionFactory connFactory, ISqlRepository repo, GoogleUtil googleUtil)
        {
            _connFactory = connFactory;
            _repo = repo;
            _googleUtil = googleUtil;
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

                var rows = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
                if (rows.Length < 4)
                {
                    res.SetResult(ErrorStatusCode.Invalid_Error);
                    res.ResultMsg = "구글 시트 데이터가 올바르지 않습니다.";
                    return res;
                }

                for (int i = 3; i < rows.Length; i++)
                {
                    string[] columns = rows[i].Split(',');
                    string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);

                    var req = new RegisterRequestDto()
                    {
                        area = columns[1],
                        option = ConvertOption(columns[3]),
                        day = ConvertDay(columns[4]),
                        buyer = columns[5],
                        attender = columns[6],
                        phone = columns[7].Replace("-", ""),
                        gender = ConvertGender(columns[8]),
                        age = ToShortOrZero(columns[9]),
                        church = columns[10],
                        local = columns[11],
                        denom = columns[12],
                        count = ToShortOrZero(columns[13]),
                        memo = columns[14],
                        notionSmsYn = columns[15] == "O" ? "Y" : "N",
                    };

                    var data = await _repo.IC26DataDao.GenerateRegisterAsync(db, req, uniqueId);
                    if (data < 0)
                    {
                        res.SetResult(ErrorStatusCode.Invalid_Error);
                        res.ResultMsg = "컨퍼런스 참가 등록 실패";
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
                        newBelieverYn = "N",
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
                    return 1;
                case "[수]":
                    return 2;
                case "[목]":
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

            if (new[] { "남성", "남자" }.Contains(gender))
                return "M";

            if (new[] { "여성", "여자" }.Contains(gender))
                return "F";

            if (new[] { "남여", "남녀" }.Contains(gender))
                return "M/F";

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
}
