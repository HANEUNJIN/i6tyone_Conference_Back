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
                              FROM IC26_Data
                            ";

            return await db.ExecuteScalarAsync<int>(query);
        }

        public async Task<int> GenerateRegisterAsync(DbSession db, RegisterRequestDto req, int seq, string iC26UniqueId)
        {
            try
            {
                string query = @"
                                INSERT INTO IC26_Data (
                                    IC26_No,
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
                                    IC26_UniqueId,
                                    IC26_Attend,
                                    IC26_CreateQR,
                                    IC26_SMS
                                )
                                VALUES (
                                    @nextNo,
                                    @iC26Option,
                                    @iC26Day,
                                    @iC26Buyer,
                                    @iC26Attender,
                                    @iC26Phone,
                                    @iC26Gender,
                                    @iC26Age,
                                    @iC26Church,
                                    @iC26Local,
                                    @iC26Denom,
                                    @iC26Count,
                                    @iC26Area,
                                    @iC26Memo,
                                    @iC26UniqueId,
                                    @iC26Attend,
                                    @iC26CreateQR,
                                    @iC26SMS
                                );
                                ";

                var parameters = new DynamicParameters(req);
                parameters.Add("@nextNo", seq);
                parameters.Add("@iC26UniqueId", iC26UniqueId);

                return await db.ExecuteAsync(query, parameters);
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Insert_Error, $"[DB INSERT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<List<IC26DataRecord>> GetRegisterInfoAsync(DbSession db, RegisterInfoRequestDto req)
        {
            try
            {
                string query = @"
                                 SELECT IC26_No AS IC26No,
                                        IC26_Option AS IC26Option,
                                        IC26_Day AS IC26Day,
                                        IC26_Buyer AS IC26Buyer,
                                        IC26_Attender AS IC26Attender,
                                        IC26_Phone AS IC26Phone,
                                        IC26_Gender AS IC26Gender,
                                        IC26_Age AS IC26Age,
                                        IC26_Church AS IC26Church,
                                        IC26_Local AS IC26Local,
                                        IC26_Denom AS IC26Denom,
                                        IC26_Count AS IC26Count,
                                        IC26_Area AS IC26Area,
                                        IC26_Memo AS IC26Memo,
                                     -- IC26_UniqueId AS IC26UniqueId,
                                        IC26_Attend AS IC26Attend,
                                        IC26_CreateQR AS IC26CreateQR,
                                        IC26_SMS AS IC26SMS
                                   FROM IC26_Data
                                  WHERE IC26_Buyer = @IC26Buyer
                                ";

                if (!string.IsNullOrWhiteSpace(req.iC26Phone))
                    query += "      AND IC26_Phone = @IC26Phone;";

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
                                 SELECT IC26_No AS IC26No,
                                        IC26_Option AS IC26Option,
                                        IC26_Day AS IC26Day,
                                        IC26_Buyer AS IC26Buyer,
                                        IC26_Attender AS IC26Attender,
                                        IC26_Phone AS IC26Phone,
                                        IC26_Gender AS IC26Gender,
                                        IC26_Age AS IC26Age,
                                        IC26_Church AS IC26Church,
                                        IC26_Local AS IC26Local,
                                        IC26_Denom AS IC26Denom,
                                        IC26_Count AS IC26Count,
                                        IC26_Area AS IC26Area,
                                        IC26_Memo AS IC26Memo,
                                        IC26_UniqueId AS IC26UniqueId,
                                        IC26_Attend AS IC26Attend,
                                        IC26_CreateQR AS IC26CreateQR,
                                        IC26_SMS AS IC26SMS
                                   FROM IC26_Data;
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

        public async Task<bool> CheckAttendanceAsync(DbSession db, string iC26UniqueId)
        {
            try
            {
                string query = @"
                                UPDATE IC26_Data
                                   SET IC26_Attend = '1'
                                 WHERE IC26_UniqueId = @iC26UniqueId
                                ";

                return await db.ExecuteAsync(query, new { iC26UniqueId }) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<IC26DataRecord> GetRegisterDetailAsync(DbSession db, string iC26UniqueId)
        {
            try
            {
                string query = @"
                                SELECT IC26_Option AS IC26Option,
                                       IC26_Day AS IC26Day,
                                       IC26_Buyer AS IC26Buyer,
                                       IC26_Count AS IC26Count,
                                       IC26_Area AS IC26Area
                                  FROM IC26_Data
                                 WHERE IC26_UniqueId = @iC26UniqueId;
                                ";

                return await db.QuerySingleAsync<IC26DataRecord>(query, new { iC26UniqueId });
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<BraceletRequestDto> GetBraceletAsync(DbSession db)
        {
            try
            {
                string query = @"
                                SELECT
                                    SUM(IC26_Count) AS ticket
                                FROM IC26_Data;
                                ";

                return await db.QuerySingleAsync<BraceletRequestDto>(query);
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
                                FROM IC26_Data;
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
                                    SUM(CASE WHEN IC26_Area = 'J' THEN 1 ELSE 0 END) AS J,
                                    SUM(CASE WHEN IC26_Area = 'K' THEN 1 ELSE 0 END) AS K
                                FROM IC26_Data;
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
                                  FROM IC26_Data;
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
                                    SUM(CASE WHEN IC26_CreateQR = 0 THEN 1 ELSE 0 END) AS QRNotCreated,
                                    SUM(CASE WHEN IC26_CreateQR = 1 THEN 1 ELSE 0 END) AS QRCreated,
                                    SUM(CASE WHEN IC26_SMS = 0 THEN 1 ELSE 0 END) AS SMSNotSent,
                                    SUM(CASE WHEN IC26_SMS = 1 THEN 1 ELSE 0 END) AS SMSSent
                                FROM IC26_Data;
                                ";

                return await db.QuerySingleAsync<SendRequestDto>(query);
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }
    }
}
