using System.Data.Common;

namespace eGhis_WebService_Core.Infrastructure.Db
{
    public static class DbConnectionFactoryExtensions
    {
        public static async Task<DbSessionScope> OpenSessionAsync(this IDbConnectionFactory factory, CancellationToken ct = default)
        {
            var conn = (DbConnection)factory.CreateConnection();
            await conn.OpenAsync(ct);
            var db = new DbSession(conn, transaction: null, ct: ct);
            return new DbSessionScope(conn, db);
        }
    }
}
