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
                                    newBelieverYn,
                                    IC26_Count,
                                    IC26_Area,
                                    IC26_Memo,
                                    IC26_UniqueId,
                                    notion_sms_yn,
                                    applyYmd
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
                                    @newBelieverYn,
                                    @Count,
                                    @Area,
                                    @Memo,
                                    @UniqueId,
                                    @NotionSmsYn,
                                    @ApplyYmd
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

        public async Task<int> OnSiteGenerateRegisterAsync(DbSession db, RegisterRequestDto req, string iC26UniqueId)
        {
            try
            {
                string query = @"
                                INSERT INTO isaiah61co_conf.dbo.IC26_OnSiteData (
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
                                    newBelieverYn,
                                    IC26_Count,
                                    IC26_Area,
                                    IC26_Memo,
                                    IC26_UniqueId,
                                    notion_sms_yn,
                                    applyYmd
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
                                    'N',
                                    @Count,
                                    '',
                                    '',
                                    @UniqueId,
                                    'N',
                                    @applyYmd
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
                                       newBelieverYn = @newBelieverYn,
                                       IC26_Count = @count,
                                       IC26_Area = @area,
                                       IC26_Attend = @attend,
                                       IC26_Memo = @memo,
                                       notion_sms_yn = @notionSmsYn
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
                                newBelieverYn AS newBelieverYn,
                                IC26_Count AS Count,
                                IC26_Area AS Area,
                                IC26_Memo AS Memo,
                                IC26_UniqueId AS UniqueId,
                                IC26_Attend AS Attend,
                                IC26_CreateQR AS CreateQR,
                                IC26_SMS AS SMS,
                                notion_sms_yn AS NotionSmsYn,
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
                                        OR IC26_Denom LIKE CONCAT('%', @keyword, '%')
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

        public async Task<List<IC26DataRecord>> GetRegisterListAsync(DbSession db)
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
                                newBelieverYn AS newBelieverYn,
                                IC26_Count AS Count,
                                IC26_Area AS Area,
                                IC26_Memo AS Memo,
                                IC26_UniqueId AS UniqueId,
                                IC26_Attend AS Attend,
                                IC26_CreateQR AS CreateQR,
                                IC26_SMS AS SMS,
                                notion_sms_yn AS NotionSmsYn,
                                COUNT(*) OVER() AS Total
                           FROM isaiah61co_conf.dbo.IC26_Data
                       ORDER BY IC26_No DESC;
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

        public async Task<IC26DataRecord> GetRegisterDetailInfoAsync(DbSession db, string uniqueId)
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
                                newBelieverYn AS newBelieverYn,
                                IC26_Count AS Count,
                                IC26_Area AS Area,
                                IC26_Memo AS Memo,
                                IC26_Attend AS Attend,
                                IC26_CreateQR AS CreateQR,
                                IC26_SMS AS SMS,
                                notion_sms_yn AS NotionSmsYn,
                                COUNT(*) OVER() AS Total
                           FROM isaiah61co_conf.dbo.IC26_Data
                          WHERE IC26_UniqueId = @uniqueId;
                        ";

                return await db.QuerySingleAsync<IC26DataRecord>(query, new { uniqueId });
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
                                        newBelieverYn AS newBelieverYn,
                                        IC26_Count AS Count,
                                        IC26_Area AS Area,
                                        IC26_Memo AS Memo,
                                        IC26_UniqueId AS UniqueId,
                                        IC26_Attend AS Attend,
                                        IC26_CreateQR AS CreateQR,
                                        IC26_SMS AS SMS,
                                        notion_sms_yn AS NotionSmsYn
                                   FROM isaiah61co_conf.dbo.IC26_Data
                                  WHERE delYn = 'N';
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
                                        newBelieverYn AS newBelieverYn,
                                        IC26_Count AS Count,
                                        IC26_Area AS Area,
                                        IC26_Memo AS Memo,
                                        IC26_UniqueId AS UniqueId,
                                        IC26_Attend AS Attend,
                                        IC26_CreateQR AS CreateQR,
                                        IC26_SMS AS SMS,
                                        notion_sms_yn AS NotionSmsYn
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

        public async Task<List<DateInfo>> GetDataAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT 
                                    CASE 
                                        WHEN GROUPING(IC26_Day) = 1 THEN 'total'
                                        ELSE CAST(IC26_Day AS VARCHAR(20))
                                    END AS Day,
                                    SUM(IC26_Count) AS Total
                                FROM isaiah61co_conf.dbo.IC26_Data
                                WHERE delYn = 'N'
                                GROUP BY ROLLUP (IC26_Day)
                                ORDER BY 
                                    CASE WHEN GROUPING(IC26_Day) = 1 THEN 1 ELSE 0 END,
                                    IC26_Day;
                                ";

                var result = await db.QueryAsync<DateInfo>(query);
                return result.ToList();
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Summary>> GetTicketOptionAsync(DbSession db)
        {
            try
            {
                string query = @"
                                 SELECT
                                 CASE
                                     WHEN IC26_Day IS NULL THEN 'Total'
                                     WHEN IC26_Day = 1 THEN '화'
                                     WHEN IC26_Day = 2 THEN '수'
                                     WHEN IC26_Day = 3 THEN '목'
                                     WHEN IC26_Day = 4 THEN '3-day'
                                 END AS [Day],
                     
                                 SUM(CASE WHEN IC26_Option = 1 THEN IC26_Count ELSE 0 END) AS [SuperEarly],
                                 SUM(CASE WHEN IC26_Option = 2 THEN IC26_Count ELSE 0 END) AS [Early1],
                                 SUM(CASE WHEN IC26_Option = 3 THEN IC26_Count ELSE 0 END) AS [Early2],
                                 SUM(CASE WHEN IC26_Option IN (1,2,3) THEN IC26_Count ELSE 0 END) AS [EarlyTotal],
                                 SUM(CASE WHEN IC26_Option = 4 THEN IC26_Count ELSE 0 END) AS [Regular],
                                 SUM(CASE WHEN IC26_Option = 5 THEN IC26_Count ELSE 0 END) AS [Event],
                                 SUM(CASE WHEN IC26_Option = 6 THEN IC26_Count ELSE 0 END) AS [Site],
                                 SUM(CASE WHEN IC26_Option = 7 THEN IC26_Count ELSE 0 END) AS [VIP],
                                 SUM(IC26_Count) AS [Total]
                             FROM isaiah61co_conf.dbo.IC26_Data
                             WHERE delYn = 'N'
                             GROUP BY GROUPING SETS (
                                 (IC26_Day),   -- Day별
                                 ()            -- 전체 Total
                             )
                             ORDER BY
                                 CASE
                                     WHEN IC26_Day IS NULL THEN 5
                                     WHEN IC26_Day = 1 THEN 1
                                     WHEN IC26_Day = 2 THEN 2
                                     WHEN IC26_Day = 3 THEN 3
                                     WHEN IC26_Day = 4 THEN 4
                                 END;
                                ";

                var result = await db.QueryAsync<Summary>(query);
                return result.ToList();
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
                                    SUM(CASE WHEN IC26_Area = 'A1' THEN 1 ELSE 0 END) AS A1,
                                    SUM(CASE WHEN IC26_Area = 'B1' THEN 1 ELSE 0 END) AS B1,
                                    SUM(CASE WHEN IC26_Area = 'C1' THEN 1 ELSE 0 END) AS C1,
                                    SUM(CASE WHEN IC26_Area = 'D1' THEN 1 ELSE 0 END) AS D1,
                                    SUM(CASE WHEN IC26_Area = 'A2-1' THEN 1 ELSE 0 END) AS A2_1,
                                    SUM(CASE WHEN IC26_Area = 'A2-2' THEN 1 ELSE 0 END) AS A2_2,
                                    SUM(CASE WHEN IC26_Area = 'B2-1' THEN 1 ELSE 0 END) AS B2_1,
                                    SUM(CASE WHEN IC26_Area = 'B2-2' THEN 1 ELSE 0 END) AS B2_2,
                                    SUM(CASE WHEN IC26_Area = 'C2-1' THEN 1 ELSE 0 END) AS C2_1,
                                    SUM(CASE WHEN IC26_Area = 'C2-2' THEN 1 ELSE 0 END) AS C2_2,
                                    SUM(CASE WHEN IC26_Area = 'D2-1' THEN 1 ELSE 0 END) AS D2_1,
                                    SUM(CASE WHEN IC26_Area = 'D2-2' THEN 1 ELSE 0 END) AS D2_2,
                                    SUM(CASE WHEN IC26_Area = 'E2-1' THEN 1 ELSE 0 END) AS E2_1,
                                    SUM(CASE WHEN IC26_Area = 'E2-2' THEN 1 ELSE 0 END) AS E2_2,
                                    SUM(CASE WHEN IC26_Area = 'F2-1' THEN 1 ELSE 0 END) AS F2_1,
                                    SUM(CASE WHEN IC26_Area = 'F2-2' THEN 1 ELSE 0 END) AS F2_2,
                                    SUM(CASE WHEN IC26_Area = 'G2-1' THEN 1 ELSE 0 END) AS G2_1,
                                    SUM(CASE WHEN IC26_Area = 'G2-2' THEN 1 ELSE 0 END) AS G2_2,
                                    SUM(CASE WHEN IC26_Area = '유아&장애' THEN 1 ELSE 0 END) AS 유아_장애
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
                                SELECT sum(IC26_Count) AS total_users
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

        public async Task<List<SendInfo>> GetSendAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT
                                    v.YN,
                                    SUM(CASE WHEN IC26_Attend = v.YN THEN IC26_Count ELSE 0 END) AS Attend,
                                    SUM(CASE WHEN IC26_SMS = v.YN THEN IC26_Count ELSE 0 END) AS QRSms,
                                    SUM(CASE WHEN notion_sms_yn = v.YN THEN IC26_Count ELSE 0 END) AS NotionSms
                                FROM isaiah61co_conf.dbo.IC26_Data d
                                CROSS APPLY (VALUES ('Y'), ('N')) v(YN)
                                WHERE delYn = 'N'
                                GROUP BY v.YN;
                                ";

                var result = await db.QueryAsync<SendInfo>(query);
                return result.ToList();
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
                                   SET IC26_CreateQR = 'Y',
                                       IC26_SMS = 'Y'
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
