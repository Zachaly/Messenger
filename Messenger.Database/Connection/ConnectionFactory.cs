using Microsoft.Extensions.Configuration;
using System.Data;

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
        {
            throw new NotImplementedException();
        }
    }
}
