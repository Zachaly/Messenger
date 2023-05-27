using System.Data;

namespace Messenger.Database.Connection
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
        IDbConnection GetMasterConnection();
    }
}
