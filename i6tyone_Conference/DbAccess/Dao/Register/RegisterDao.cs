using Dapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Models.Db;
using eGhis_WebService_Core.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;
using static Dapper.SqlMapper;

namespace eGhis_WebService_Core.DbAccess.Dao.PmLicenseNew
{
    public class RegisterDao : IRegisterDao
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
    }
}
