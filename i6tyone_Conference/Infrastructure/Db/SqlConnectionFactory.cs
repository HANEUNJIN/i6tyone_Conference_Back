using Microsoft.Extensions.Options;
using System.Data;
using eGhis_WebService_Core.Models.Config;
using Microsoft.Data.SqlClient;

namespace eGhis_WebService_Core.Infrastructure.Db
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IOptions<ConnectionString> options)
        {
            _connectionString = options.Value.ClinicConn ?? throw new InvalidOperationException("Connection string is null");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
