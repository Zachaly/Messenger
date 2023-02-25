using System.Data;

namespace Messenger.Database.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IDbConnection GetConnection()
        {
            throw new NotImplementedException();
        }
    }
}
