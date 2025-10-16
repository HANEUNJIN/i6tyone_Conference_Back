using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.DbAccess.Dao.PmLicenseNew;
using i6tyone_Conference.DbAccess.Dao.Conference;

namespace eGhis_WebService_Core.Repositories
{
    public class SqlRepository : ISqlRepository
    {
        private readonly IDbConnectionFactory _factory;

        public IRegisterDao RegisterDao { get; }

        public IConferenceDao ConferenceDao { get; }

        public SqlRepository(IDbConnectionFactory factory, IRegisterDao registerDao, IConferenceDao conferenceDao)
        {
            _factory = factory;
            RegisterDao = registerDao;
            ConferenceDao = conferenceDao;
        }
    }
}
