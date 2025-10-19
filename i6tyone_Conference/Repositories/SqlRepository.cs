using eGhis_WebService_Core.Infrastructure.Db;
using i6tyone_Conference.DbAccess.Dao;

namespace eGhis_WebService_Core.Repositories
{
    public class SqlRepository : ISqlRepository
    {
        private readonly IDbConnectionFactory _factory;

        public IIC26DataDao IC26DataDao { get; }

        public SqlRepository(IDbConnectionFactory factory, IIC26DataDao iC26DataDao)
        {
            _factory = factory;
            IC26DataDao = iC26DataDao;
        }
    }
}
