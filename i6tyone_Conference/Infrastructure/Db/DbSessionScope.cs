using System.Data.Common;

namespace eGhis_WebService_Core.Infrastructure.Db
{
    public readonly struct DbSessionScope : IAsyncDisposable
    {
        public DbConnection Connection { get; }
        public DbSession Session { get; }

        public DbSessionScope(DbConnection connection, DbSession session)
        {
            Connection = connection;
            Session = session;
        }

        public ValueTask DisposeAsync() => Connection.DisposeAsync();
    }
}
