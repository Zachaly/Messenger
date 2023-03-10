using Dapper;
using Messenger.Database.Connection;
using Messenger.Domain.Entity;
using Moq;
using System.Data.SqlClient;

namespace Messenger.Tests.Integration.Database
{
    public abstract class DatabaseTest : IDisposable
    {
        protected string _connectionString = "Server=localhost;Database=MessengerTest;Trusted_Connection=True;";
        protected List<string> _teardownQueries = new List<string>();
        protected readonly IConnectionFactory _connectionFactory;

        public DatabaseTest()
        {
            var connectionFactory = new Mock<IConnectionFactory>();
            connectionFactory.Setup(x => x.GetConnection())
                .Returns(new SqlConnection(_connectionString));

            _connectionFactory = connectionFactory.Object;
        }

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

        protected async Task<IEnumerable<T>> GetAllFromDatabase<T>(string table)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<T>($"SELECT * FROM [{table}]");
            }
        }

        protected async Task InsertUsersToDatabase(IEnumerable<User> users)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var user in users)
                {
                    await connection.QueryAsync("INSERT INTO [User]([Login], [Name], [PasswordHash]) VALUES(@Login, @Name, @PasswordHash)", user);
                }
            }
        }

        protected async Task InsertFriendsToDatabase(IEnumerable<Friend> friends)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var friend in friends)
                {
                    await connection.QueryAsync("INSERT INTO [Friend]([User1Id], [User2Id]) VALUES(@User1Id, @User2Id)", friend);
                }
            }
        }
    }
}
