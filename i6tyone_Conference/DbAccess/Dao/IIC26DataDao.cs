using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Models.Db;
using eGhis_WebService_Core.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Statistics;

namespace i6tyone_Conference.DbAccess.Dao
{
    public interface IIC26DataDao : IDaoMarker
    {
        public Task<int> GetSeqAsync(DbSession db);
        public Task<int> GenerateRegisterAsync(DbSession db, RegisterRequestDto req, int seq, string iC26UniqueId);
        public Task<List<IC26DataRecord>> GetRegisterInfoAsync(DbSession db, RegisterInfoRequestDto req);
        public Task<List<IC26DataRecord>> GenerateRegisterQRCodeAsync(DbSession db);
        public Task<IC26DataRecord> GenerateRegisterQRCodeSingleAsync(DbSession db, IssuanceRequestDto req);
        public Task<bool> CheckAttendanceAsync(DbSession db, string uniqueId);
        public Task<IC26DataRecord> GetRegisterDetailAsync(DbSession db, string iC26UniqueId);
        public Task<BraceletRequestDto> GetBraceletAsync(DbSession db);
        public Task<DateRequestDto> GetDataAsync(DbSession db);
        public Task<AreaRequestDto> GetAreaAsync(DbSession db);
        public Task<RegistrationRequestDto> GetRegistrationAsync(DbSession db);
        public Task<SendRequestDto> GetSendAsync(DbSession db);
    }
}
