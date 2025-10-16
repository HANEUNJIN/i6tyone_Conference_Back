using Microsoft.Extensions.Options;
using System.Data;
using eGhis_WebService_Core.Models.Config;
using MySqlConnector;

namespace eGhis_WebService_Core.Infrastructure.Db
{
    public class MysqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MysqlConnectionFactory(IOptions<ConnectionString> options)
        {
            _connectionString = options.Value.ClinicConn ?? throw new InvalidOperationException("Connection string is null");
        }

        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}
