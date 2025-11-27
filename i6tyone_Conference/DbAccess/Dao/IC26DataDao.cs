using Dapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Models.Db;
using eGhis_WebService_Core.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Statistics;
using static Dapper.SqlMapper;

namespace i6tyone_Conference.DbAccess.Dao
{
    public class IC26DataDao : IIC26DataDao
    {
        public async Task<int> GetSeqAsync(DbSession db)
        {
            string query = @"
                            SELECT IFNULL(MAX(IC26_No), 0) + 1
                              FROM isaiah61co_conf.dbo.IC26_Data
                            ";

            return await db.ExecuteScalarAsync<int>(query);
        }

        public async Task<int> GenerateRegisterAsync(DbSession db, RegisterRequestDto req, string iC26UniqueId)
        {
            try
            {
                string query = @"
                                INSERT INTO isaiah61co_conf.dbo.IC26_Data (
                                    IC26_Option,
                                    IC26_Day,
                                    IC26_Buyer,
                                    IC26_Attender,
                                    IC26_Phone,
                                    IC26_Gender,
                                    IC26_Age,
                                    IC26_Church,
                                    IC26_Local,
                                    IC26_Denom,
                                    IC26_Count,
                                    IC26_Area,
                                    IC26_Memo,
                                    IC26_UniqueId
                                )
                                VALUES (
                                    @Option,
                                    @Day,
                                    @Buyer,
                                    @Attender,
                                    @Phone,
                                    @Gender,
                                    @Age,
                                    @Church,
                                    @Local,
                                    @Denom,
                                    @Count,
                                    @Area,
                                    @Memo,
                                    @UniqueId
                                );
                                ";

                var parameters = new DynamicParameters(req);
                parameters.Add("@UniqueId", iC26UniqueId);

                return await db.ExecuteAsync(query, parameters);
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Insert_Error, $"[DB INSERT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateRegisterAsync(DbSession db, RegisterUpdateRequestDto req, string uniqueId)
        {
            try
            {
                string query = @"
                                UPDATE isaiah61co_conf.dbo.IC26_Data
                                   SET
                                       IC26_Option = @option,
                                       IC26_Day = @day,
                                       IC26_Buyer = @buyer,
                                       IC26_Attender = @attender,
                                       IC26_Phone = @phone,
                                       IC26_Gender = @gender,
                                       IC26_Age = @age,
                                       IC26_Church = @church,
                                       IC26_Local = @local,
                                       IC26_Denom = @denom,
                                       IC26_Count = @count,
                                       IC26_Area = @area,
                                       IC26_Memo = @memo
                                 WHERE IC26_UniqueId = @uniqueId
                                   AND delYn = 'N';
                                ";

                var parameters = new DynamicParameters(req);
                parameters.Add("@uniqueId", uniqueId);

                return await db.ExecuteAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Insert_Error, $"[DB INSERT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DisposeRegisterAsync(DbSession db, string uniqueId)
        {
            try
            {
                string query = @"
                                UPDATE isaiah61co_conf.dbo.IC26_Data
                                   SET delYn = 'Y',
                                       delYmd = GETDATE()
                                 WHERE IC26_UniqueId = @UniqueId
                                ";

                return await db.ExecuteAsync(query, new { uniqueId }) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteRegisterAsync(DbSession db, string uniqueId)
        {
            try
            {
                string query = @"
                                DELETE FROM isaiah61co_conf.dbo.IC26_Data
                                 WHERE IC26_UniqueId = @UniqueId;
                                ";

                return await db.ExecuteAsync(query, new { uniqueId }) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<List<IC26DataRecord>> GetRegisterInfoAsync(DbSession db, RegisterInfoRequestDto req)
        {
            try
            {
                string query = @"
                         SELECT IC26_No AS No,
                                IC26_Option AS [Option],
                                IC26_Day AS Day,
                                IC26_Buyer AS Buyer,
                                IC26_Attender AS Attender,
                                IC26_Phone AS Phone,
                                IC26_Gender AS Gender,
                                IC26_Age AS Age,
                                IC26_Church AS Church,
                                IC26_Local AS Local,
                                IC26_Denom AS Denom,
                                IC26_Count AS Count,
                                IC26_Area AS Area,
                                IC26_Memo AS Memo,
                                IC26_UniqueId AS UniqueId,
                                IC26_Attend AS Attend,
                                IC26_CreateQR AS CreateQR,
                                IC26_SMS AS SMS,
                                COUNT(*) OVER() AS Total
                           FROM isaiah61co_conf.dbo.IC26_Data
                          WHERE 1=1
                            AND delYn = 'N'
                        ";

                if (req.option != null && req.option.Any())
                    query += "      AND IC26_Option IN @option";

                if (req.day != null && req.day.Any())
                    query += "      AND IC26_Day IN @day";

                if (!string.IsNullOrWhiteSpace(req.keyword))
                {
                    query += @"
                                    AND (
                                           IC26_Buyer LIKE CONCAT('%', @keyword, '%')
                                        OR IC26_Attender LIKE CONCAT('%', @keyword, '%')
                                        OR IC26_Phone LIKE CONCAT('%', @keyword, '%')
                                        OR REPLACE(IC26_Church, ' ', '') LIKE '%' + REPLACE(@keyword, ' ', '') + '%'
                                        OR IC26_Local LIKE CONCAT('%', @keyword, '%')
                                    )";
                }

                if (req.area != null && req.area.Any())
                    query += "      AND IC26_Area IN @area";

                query += @"
                        ORDER BY IC26_No DESC
                        OFFSET (@pageNum - 1) * @pageSize ROWS
                        FETCH NEXT @pageSize ROWS ONLY;
                    ";

                var result = await db.QueryAsync<IC26DataRecord>(query, req);
                return result.ToList();
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<List<IC26DataRecord>> GenerateRegisterQRCodeAsync(DbSession db)
        {
            try
            {
                string query = @"
                                 SELECT IC26_No AS No,
                                        IC26_Option AS [Option],
                                        IC26_Day AS Day,
                                        IC26_Buyer AS Buyer,
                                        IC26_Attender AS Attender,
                                        IC26_Phone AS Phone,
                                        IC26_Gender AS Gender,
                                        IC26_Age AS Age,
                                        IC26_Church AS Church,
                                        IC26_Local AS Local,
                                        IC26_Denom AS Denom,
                                        IC26_Count AS Count,
                                        IC26_Area AS Area,
                                        IC26_Memo AS Memo,
                                        IC26_UniqueId AS UniqueId,
                                        IC26_Attend AS Attend,
                                        IC26_CreateQR AS CreateQR,
                                        IC26_SMS AS SMS
                                   FROM isaiah61co_conf.dbo.IC26_Data
                                    AND delYn = 'N';
                                ";

                var result = await db.QueryAsync<IC26DataRecord>(query);
                return result.ToList();
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<IC26DataRecord> GenerateRegisterQRCodeSingleAsync(DbSession db, IssuanceRequestDto req)
        {
            try
            {
                string query = @"
                                 SELECT IC26_No AS No,
                                        IC26_Option AS [Option],
                                        IC26_Day AS Day,
                                        IC26_Buyer AS Buyer,
                                        IC26_Attender AS Attender,
                                        IC26_Phone AS Phone,
                                        IC26_Gender AS Gender,
                                        IC26_Age AS Age,
                                        IC26_Church AS Church,
                                        IC26_Local AS Local,
                                        IC26_Denom AS Denom,
                                        IC26_Count AS Count,
                                        IC26_Area AS Area,
                                        IC26_Memo AS Memo,
                                        IC26_UniqueId AS UniqueId,
                                        IC26_Attend AS Attend,
                                        IC26_CreateQR AS CreateQR,
                                        IC26_SMS AS SMS
                                   FROM isaiah61co_conf.dbo.IC26_Data
                                  WHERE IC26_Buyer = @Buyer
                                    AND IC26_Phone = @Phone
                                    AND delYn = 'N';
                                ";

                return await db.QuerySingleAsync<IC26DataRecord>(query, req);
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<int> CheckAttendanceAsync(DbSession db, string uniqueId)
        {
            try
            {
                string query = @"
                                UPDATE isaiah61co_conf.dbo.IC26_Data
                                   SET IC26_Attend = 'Y'
                                 WHERE IC26_UniqueId = @UniqueId
                                   AND IC26_Attend = 'N'
                                   AND delYn = 'N';
                                ";

                return await db.ExecuteAsync(query, new { uniqueId });

            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<IC26DataRecord> GetRegisterDetailAsync(DbSession db, string uniqueId)
        {
            try
            {
                string query = @"
                                SELECT IC26_Option AS [Option],
                                       IC26_Buyer AS Buyer,
                                       IC26_Attender AS Attender,
                                       IC26_Church AS church,
                                       IC26_Count AS Count,
                                       IC26_Area AS Area,
                                       IC26_Attend AS Attend
                                  FROM isaiah61co_conf.dbo.IC26_Data
                                 WHERE IC26_UniqueId = @UniqueId
                                   AND delYn = 'N';
                                ";

                return await db.QuerySingleAsync<IC26DataRecord>(query, new { uniqueId });
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<TicketRequestDto> GetTicketAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT
                                    SUM(IC26_Count) AS ticket
                                FROM isaiah61co_conf.dbo.IC26_Data
                               WHERE delYn = 'N';
                                ";

                return await db.QuerySingleAsync<TicketRequestDto>(query);
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<DateRequestDto> GetDataAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT
                                    SUM(CASE WHEN IC26_Day IN (1, 4) THEN 1 ELSE 0 END) AS Day1Count,
                                    SUM(CASE WHEN IC26_Day IN (2, 4) THEN 1 ELSE 0 END) AS Day2Count,
                                    SUM(CASE WHEN IC26_Day IN (3, 4) THEN 1 ELSE 0 END) AS Day3Count
                                FROM isaiah61co_conf.dbo.IC26_Data
                               WHERE delYn = 'N';
                                ";

                return await db.QuerySingleAsync<DateRequestDto>(query);
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<AreaRequestDto> GetAreaAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT
                                    SUM(CASE WHEN IC26_Area = 'A' THEN 1 ELSE 0 END) AS A,
                                    SUM(CASE WHEN IC26_Area = 'B' THEN 1 ELSE 0 END) AS B,
                                    SUM(CASE WHEN IC26_Area = 'C' THEN 1 ELSE 0 END) AS C,
                                    SUM(CASE WHEN IC26_Area = 'D' THEN 1 ELSE 0 END) AS D,
                                    SUM(CASE WHEN IC26_Area = 'E' THEN 1 ELSE 0 END) AS E,
                                    SUM(CASE WHEN IC26_Area = 'F' THEN 1 ELSE 0 END) AS F,
                                    SUM(CASE WHEN IC26_Area = 'G' THEN 1 ELSE 0 END) AS G,
                                    SUM(CASE WHEN IC26_Area = 'H' THEN 1 ELSE 0 END) AS H,
                                    SUM(CASE WHEN IC26_Area = 'I' THEN 1 ELSE 0 END) AS I,
                                    SUM(CASE WHEN IC26_Area = 'J' THEN 1 ELSE 0 END) AS J
                                FROM isaiah61co_conf.dbo.IC26_Data
                               WHERE delYn = 'N';
                                ";

                return await db.QuerySingleAsync<AreaRequestDto>(query);
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<RegistrationRequestDto> GetRegistrationAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT count(*) AS total_users
                                  FROM isaiah61co_conf.dbo.IC26_Data
                                 WHERE delYn = 'N';
                                ";

                return await db.QuerySingleAsync<RegistrationRequestDto>(query);
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<SendRequestDto> GetSendAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT
                                    SUM(CASE WHEN IC26_CreateQR = 'N' THEN 1 ELSE 0 END) AS QRNotCreated,
                                    SUM(CASE WHEN IC26_CreateQR = 'Y' THEN 1 ELSE 0 END) AS QRCreated,
                                    SUM(CASE WHEN IC26_SMS = 'N' THEN 1 ELSE 0 END) AS SMSNotSent,
                                    SUM(CASE WHEN IC26_SMS = 'Y' THEN 1 ELSE 0 END) AS SMSSent,
                                    SUM(CASE WHEN IC26_Attend = 'Y' THEN 1 ELSE 0 END) AS AttendCount,
                                    SUM(CASE WHEN IC26_Attend = 'Y' THEN 1 ELSE 0 END) AS NotAttendCount
                                FROM isaiah61co_conf.dbo.IC26_Data
                               WHERE delYn = 'N';
                                ";

                return await db.QuerySingleAsync<SendRequestDto>(query);
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CheckCreateQRAsync(DbSession db, string uniqueId)
        {
            try
            {
                string query = @"
                                UPDATE isaiah61co_conf.dbo.IC26_Data
                                   SET IC26_CreateQR = 'Y'
                                 WHERE IC26_UniqueId = @UniqueId
                                   AND delYn = 'N';
                                ";

                return await db.ExecuteAsync(query, new { uniqueId }) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ClearAttendAsync(DbSession db)
        {
            try
            {
                string query = @"
                                UPDATE isaiah61co_conf.dbo.IC26_Data
                                   SET IC26_Attend = 'N'
                                 WHERE IC26_Attend = 'Y'
                                ";

                return await db.ExecuteAsync(query) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ClearCreateQRAsync(DbSession db)
        {
            try
            {
                string query = @"
                                UPDATE isaiah61co_conf.dbo.IC26_Data
                                   SET IC26_CreateQR = 'N'
                                 WHERE IC26_CreateQR = 'Y'
                                ";

                return await db.ExecuteAsync(query) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ClearSmsAsync(DbSession db)
        {
            try
            {
                string query = @"
                                UPDATE isaiah61co_conf.dbo.IC26_Data
                                   SET IC26_SMS = 'N'
                                 WHERE IC26_SMS = 'Y'
                                ";

                return await db.ExecuteAsync(query) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }
    }
}
