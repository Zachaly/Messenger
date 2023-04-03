using Dapper;
using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.User.Request;
using System.Data;
using System.Data.SqlClient;

namespace Messenger.Tests.Integration.Database
{
    public class UserRepositoryTests : DatabaseTest
    {
        private readonly UserRepository _repository;

        public UserRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [User]");

            _repository = new UserRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetUsersAsync()
        {
            var users = FakeDataFactory.CreateUsers(4);

            await InsertUsersToDatabase(users);

            var request = new GetUserRequest { PageSize = 2, PageIndex = 1 };

            var res = await _repository.GetAsync(request);

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

            var res = await _repository.GetByLoginAsync(Login);

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

            var id = await _repository.InsertAsync(user);

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

        [Fact]
        public async Task GetUserByIdAsync()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(5));

            var user = (await GetAllFromDatabase<User>("User")).First();

            var res = await _repository.GetByIdAsync(user.Id);

            Assert.Equal(user.Id, res.Id);
            Assert.Equal(user.Name, res.Name);
        }

        [Fact]
        public async Task UpdateUserAsync()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(5));

            var userId = (await GetAllFromDatabase<User>("User")).Last().Id;

            var request = new UpdateUserRequest { Id = userId, Name = "updated name", ProfileImage = "image" };

            await _repository.UpdateAsync(request);

            var user = (await GetAllFromDatabase<User>("User")).First(x => x.Id == userId);

            Assert.Equal(request.Name, user.Name);
            Assert.Equal(request.ProfileImage, user.ProfileImage);
        }

        [Fact]
        public async Task GetEntityByIdAsync()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(5));

            var user = (await GetAllFromDatabase<User>("User")).Last();

            var res = await _repository.GetEntityByIdAsync(user.Id);

            Assert.IsType<User>(res);
            Assert.Equal(user.Name, res.Name);
            Assert.Equal(user.Login, res.Login);
            Assert.Equal(user.ProfileImage, res.ProfileImage);
        }
    }
}
