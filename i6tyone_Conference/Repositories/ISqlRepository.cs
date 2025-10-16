using eGhis_WebService_Core.DbAccess.Dao.PmLicenseNew;
using i6tyone_Conference.DbAccess.Dao.Conference;

namespace eGhis_WebService_Core.Repositories
{
    public interface ISqlRepository
    {
        IRegisterDao RegisterDao { get; }
        IConferenceDao ConferenceDao { get; }
    }
}
