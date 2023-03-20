using Messenger.Application;
using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class UserFactoryTests
    {
        private readonly UserFactory _factory;

        public UserFactoryTests()
        {
            _factory = new UserFactory();
        }

        [Fact]
        public void Create_Creates_Proper_Entity()
        {
            var request = new AddUserRequest
            {
                Login = "logg",
                Name = "user",
            };

            const string Hash = "haaaaash";

            var user = _factory.Create(request, Hash);

            Assert.Equal(Hash, user.PasswordHash);
            Assert.Equal(request.Login, user.Login);
            Assert.Equal(request.Name, user.Name);
        }

        [Fact]
        public void CreateLoginResponse_With_Entity_Creates_Proper_Model()
        {
            var user = new User
            {
                Id = 1,
                Login = "login",
                Name = "user",
            };

            const string Token = "token";

            var response = _factory.CreateLoginResponse(user, Token);

            Assert.Equal(Token, response.AuthToken);
            Assert.Equal(user.Name, response.UserName);
            Assert.Equal(user.Id, response.UserId);
        }

        [Fact]
        public void CreateLoginResponse_With_Model_Creates_Proper_Model()
        {
            var user = new UserModel
            {
                Id = 1,
                Name = "user",
            };

            const string Token = "token";

            var response = _factory.CreateLoginResponse(user, Token);

            Assert.Equal(Token, response.AuthToken);
            Assert.Equal(user.Name, response.UserName);
            Assert.Equal(user.Id, response.UserId);
        }

        [Fact]
        public void CreateModel_Creates_Proper_Model()
        {
            var user = new User { Id = 1, Name = "user" };

            var model = _factory.CreateModel(user);

            Assert.Equal(user.Name, model.Name);
            Assert.Equal(user.Id, model.Id);
        }
    }
}
