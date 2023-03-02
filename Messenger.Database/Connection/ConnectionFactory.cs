using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Messenger.Database.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string? _connectionString;

        public ConnectionFactory(IConfiguration config) 
        {
            _connectionString = config["ConnectionString"];
        }

        public IDbConnection GetConnection()
            => new SqlConnection(_connectionString);
    }
}
