using Dapper;
using System.Data;
using System.Data.Common;

namespace eGhis_WebService_Core.Infrastructure.Db
{
    /// <summary>
    /// DbSession 전용 Dapper 확장 메서드 모음
    /// - 트랜잭션, 타임아웃, 취소토큰을 일관 적용
    /// - 안전한 파라미터 바인딩 전제 (DynamicParameters 권장)
    /// </summary>
    public static class DapperExt
    {
        private const int DefaultTimeout = 30; // 초

        private static CommandDefinition Cmd(
            string sql, object? p, IDbTransaction? tran, int? timeout, CancellationToken ct)
            => new(sql, parameters: p, transaction: tran, commandTimeout: timeout ?? DefaultTimeout, cancellationToken: ct);

        // ---------- 읽기 ----------

        /// <summary>
        /// 0~1건 예상. 없으면 default(T).
        /// </summary>
        public static Task<T?> QueryFirstOrDefaultAsync<T>(this DbSession db, string sql, object? p = null, int? timeout = null)
            => db.Connection.QueryFirstOrDefaultAsync<T>(Cmd(sql, p, db.Transaction, timeout, db.Cancellation));

        /// <summary>
        /// 정확히 1건 예상. 0건 또는 2건 이상이면 예외.
        /// </summary>
        public static Task<T> QuerySingleAsync<T>(this DbSession db, string sql, object? p = null, int? timeout = null)
            => db.Connection.QuerySingleAsync<T>(Cmd(sql, p, db.Transaction, timeout, db.Cancellation));

        /// <summary>
        /// 0~1건 예상. 2건 이상이면 예외.
        /// </summary>
        public static Task<T?> QuerySingleOrDefaultAsync<T>(this DbSession db, string sql, object? p = null, int? timeout = null)
            => db.Connection.QuerySingleOrDefaultAsync<T>(Cmd(sql, p, db.Transaction, timeout, db.Cancellation));

        /// <summary>
        /// 다건 목록 조회.
        /// </summary>
        public static Task<IEnumerable<T>> QueryAsync<T>(this DbSession db, string sql, object? p = null, int? timeout = null)
            => db.Connection.QueryAsync<T>(Cmd(sql, p, db.Transaction, timeout, db.Cancellation));

        /// <summary>
        /// 다중 ResultSet 처리 (QueryMultiple).
        /// </summary>
        public static async Task<TResult> QueryMultipleAsync<TResult>(
            this DbSession db,
            string sql,
            Func<SqlMapper.GridReader, Task<TResult>> reader,
            object? p = null,
            int? timeout = null)
        {
            var cmd = Cmd(sql, p, db.Transaction, timeout, db.Cancellation);
            using var grid = await db.Connection.QueryMultipleAsync(cmd);
            return await reader(grid);
        }

        // ---------- 쓰기/스칼라 ----------

        /// <summary>
        /// INSERT/UPDATE/DELETE 등 비조회.
        /// 반환값: 영향받은 행 수.
        /// </summary>
        public static Task<int> ExecuteAsync(this DbSession db, string sql, object? p = null, int? timeout = null)
            => db.Connection.ExecuteAsync(Cmd(sql, p, db.Transaction, timeout, db.Cancellation));

        /// <summary>
        /// 단일 스칼라 값 반환 (COUNT, EXISTS, ID 등).
        /// </summary>
        public static Task<T> ExecuteScalarAsync<T>(this DbSession db, string sql, object? p = null, int? timeout = null)
            => db.Connection.ExecuteScalarAsync<T>(Cmd(sql, p, db.Transaction, timeout, db.Cancellation));
    }

   
}
