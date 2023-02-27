using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Moq;
using System.Data;
using System.Data.SqlClient;

namespace Messenger.Tests.Integration.Database
{
    public class UserRepositoryTests : DatabaseTest
    {
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var connectionFactory = new Mock<IConnectionFactory>();
            connectionFactory.Setup(x => x.GetConnection())
                .Returns(new SqlConnection(_connectionString));

            _teardownQueries.Add("TRUNCATE TABLE [User]");

            _repository = new UserRepository(new SqlQueryBuilder(), connectionFactory.Object);
        }

        private Task InsertUsersToDatabase(List<User> users)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var user in users)
                {
                    connection.QueryAsync("INSERT INTO [User]([Id], [Login], [Name], [PasswordHash]) VALUES(@Id, @Login, @Name, @PasswordHash)", user);
                }
            }
            
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetUsersAsync()
        {
            var users = new List<User>
            {
                new User { Id = 1, Login = "Login1", PasswordHash = "Hasssh", Name = "name1" },
                new User { Id = 2, Login = "Login2", PasswordHash = "Hasssh", Name = "name2" },
                new User { Id = 3, Login = "Login3", PasswordHash = "Hasssh", Name = "name3" },
                new User { Id = 4, Login = "Login4", PasswordHash = "Hasssh", Name = "name4" },
            };

            await InsertUsersToDatabase(users);

            var res = await _repository.GetUsers(1, 2);

            Assert.Equivalent(users.Skip(2).Take(2).Select(x => x.Id), res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetUserByLogin()
        {
            const string Login = "Loginn";

            var user = new User { Id = 3, Login = Login, PasswordHash = "Hasssh", Name = "name3" };

            var users = new List<User>
            {
                new User { Id = 1, Login = "Login1", PasswordHash = "Hasssh", Name = "name1" },
                new User { Id = 2, Login = "Login2", PasswordHash = "Hasssh", Name = "name2" },
                user,
                new User { Id = 4, Login = "Login4", PasswordHash = "Hasssh", Name = "name4" },
            };

            await InsertUsersToDatabase(users);

            var res = await _repository.GetUserByLogin(Login);

            Assert.Equal(user.Id, res.Id);
            Assert.Equal(user.Name, res.Name);
            Assert.Equal(user.PasswordHash, res.PasswordHash);
            Assert.Equal(user.Login, res.Login);
        }

        [Fact]
        public async Task InsertUser()
        {
            var user = new User
            {
                Name = "Name",
                Login = "Login",
                PasswordHash = "Hash",
            };

            var id = await _repository.InsertUser(user);

            User test;

            using(var connection = new SqlConnection(_connectionString))
            {
                test = connection.QueryFirst<User>("SELECT * FROM [User] WHERE [Id]=@Id", new { Id = id });
            }

            Assert.Equal(id, test.Id);
            Assert.Equal(user.Name, test.Name);
            Assert.Equal(user.Login, test.Login);
            Assert.Equal(user.PasswordHash, test.PasswordHash);
        }
    }
}
