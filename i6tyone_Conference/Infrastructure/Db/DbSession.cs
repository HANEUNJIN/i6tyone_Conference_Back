using System.Data;

namespace eGhis_WebService_Core.Infrastructure.Db
{
    public sealed class DbSession : IAsyncDisposable
    {
        public DbSession(IDbConnection connection, IDbTransaction? transaction = null, CancellationToken ct = default)
        {
            Connection = connection;
            Transaction = transaction;
            Cancellation = ct;
        }

        public IDbConnection Connection { get; }
        public IDbTransaction? Transaction { get; }
        public CancellationToken Cancellation { get; }

        public DbSession With(IDbTransaction? tran) => new(Connection, tran, Cancellation);
        public DbSession With(CancellationToken ct) => new(Connection, Transaction, ct);

        public ValueTask DisposeAsync() => ValueTask.CompletedTask; // Connection/Tran은 외부에서 관리
    }
}