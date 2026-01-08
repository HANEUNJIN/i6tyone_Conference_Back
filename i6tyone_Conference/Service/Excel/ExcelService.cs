using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Auth;

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
}
