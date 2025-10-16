using Dapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Models.Db;
using eGhis_WebService_Core.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Auth;
using static Dapper.SqlMapper;

namespace eGhis_WebService_Core.DbAccess.Dao.PmLicenseNew
{
    public class RegisterDao : IRegisterDao
    {
        public async Task<int> GenerateRegisterAsync(DbSession db, RegisterRequestDto req)
        {
            try
            {
                string query = @"
                                INSERT INTO i6tyone.attendee
                                (name, email, phone_number, gender, age, church_name, region, denomination, floor_seat, consent_privacy)
                                VALUES
                                (@name, @email, @phoneNumber, @gender, @age, @churchName, @region, @denomination, @floorSeat, @consentPrivacy)
                                ";

                return await db.ExecuteAsync(query, req);
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
                                 SELECT id AS id,
                                        name AS name,
                                        email AS email,
                                        phone_number AS phoneNumber,
                                        gender AS gender,
                                        age AS age,
                                        church_name AS churchName,
                                        region AS region,
                                        denomination AS denomination,
                                        floor_seat AS floorSeat,
                                        consent_privacy AS consentPrivacy,
                                        created_at AS createdAt
                                   FROM i6tyone.attendee
                                  WHERE name = @name
                                    AND phone_number = @phoneNumber;
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
                                 SELECT id AS id,
                                        name AS name,
                                        email AS email,
                                        phone_number AS phoneNumber,
                                        gender AS gender,
                                        age AS age,
                                        church_name AS churchName,
                                        region AS region,
                                        denomination AS denomination,
                                        floor_seat AS floorSeat,
                                        consent_privacy AS consentPrivacy,
                                        created_at AS createdAt
                                   FROM i6tyone.attendee;
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

        public async Task<bool> CheckAttendanceAsync(DbSession db, string qrCodeKey)
        {
            try
            {
                string query = @"
                                UPDATE i6tyone.attendee
                                   SET consent_privacy = 'Y'
                                 WHERE id = @qrCodeKey
                                ";

                return await db.ExecuteAsync(query, new { qrCodeKey }) > 0;
            }
            catch (Exception ex)
            {
                CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.DB_Update_Error, $"[DB UPDATE ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }

        public async Task<CheckInResponseDto> GetRegisterDetailAsync(DbSession db, string qrCodeKey)
        {
            try
            {
                string query = @"
                                SELECT id AS id,
                                        name AS name,
                                        email AS email,
                                        phone_number AS phoneNumber,
                                        gender AS gender,
                                        age AS age,
                                        church_name AS churchName,
                                        region AS region,
                                        denomination AS denomination,
                                        floor_seat AS floorSeat,
                                        consent_privacy AS consentPrivacy,
                                        created_at AS createdAt
                                   FROM i6tyone.attendee
                                  WHERE id = @qrCodeKey;
                                ";

                return await db.QuerySingleAsync<CheckInResponseDto>(query, new { qrCodeKey });
            }
            catch (Exception ex)
            {
                //CommonUtil.WriteLoggerString(LoggerLevel.ERROR, ErrorStatusCode.Error, $"[DB SELECT ERROR] Failed to fetch data. Reason: {ex.Message}");
                throw;
            }
        }
    }
}
