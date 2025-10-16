using System.Data;

namespace eGhis_WebService_Core.Infrastructure.Db
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
