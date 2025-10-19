using i6tyone_Conference.DbAccess.Dao;

namespace eGhis_WebService_Core.Repositories
{
    public interface ISqlRepository
    {
        IIC26DataDao IC26DataDao { get; }
    }
}
