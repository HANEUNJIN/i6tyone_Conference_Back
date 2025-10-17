using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Models.Db;
using eGhis_WebService_Core.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Auth;

namespace eGhis_WebService_Core.DbAccess.Dao.PmLicenseNew
{
    public interface IRegisterDao : IDaoMarker
    {
        public Task<int> GenerateRegisterAsync(DbSession db, RegisterRequestDto req);
        public Task<List<IC26DataRecord>> GetRegisterInfoAsync(DbSession db, RegisterInfoRequestDto req);
        public Task<List<IC26DataRecord>> GenerateRegisterQRCodeAsync(DbSession db);
        public Task<bool> CheckAttendanceAsync(DbSession db, string iC26UniqueId);
        public Task<IC26DataRecord> GetRegisterDetailAsync(DbSession db, string iC26UniqueId);
    }
}
