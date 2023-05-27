using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Messenger.Database.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string? _connectionString;
        private readonly string? _masterConnectionString;

        public ConnectionFactory(IConfiguration config) 
        {
            _connectionString = config["ConnectionString"];
            _masterConnectionString = config["MasterConnectionString"];
        }

        public IDbConnection GetConnection()
            => new SqlConnection(_connectionString);

        public IDbConnection GetMasterConnection()
            => new SqlConnection(_masterConnectionString);
    }
}
