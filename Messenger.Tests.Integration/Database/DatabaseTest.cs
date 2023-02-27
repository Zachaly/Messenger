using Dapper;
using System.Data.SqlClient;

namespace Messenger.Tests.Integration.Database
{
    public abstract class DatabaseTest : IDisposable
    {
        protected string _connectionString = "Server=localhost;Database=MessengerTest;Trusted_Connection=True;";
        protected List<string> _teardownQueries = new List<string>();

        public void Dispose()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var query in _teardownQueries)
                {
                    connection.Query(query);
                }
            }
        }
    }
}
