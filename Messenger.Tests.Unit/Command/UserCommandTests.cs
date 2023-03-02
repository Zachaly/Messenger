using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.Response;
using Messenger.Models.User;
using Messenger.Models.User.Request;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class UserCommandTests
    {
        [Theory]
        [InlineData(0, 5)]
        [InlineData(null, 5)]
        [InlineData(0, null)]
        [InlineData(null, null)]
        public async Task GetUsersQuery(int? pageIndex, int? pageSize)
        {
            var users = new List<UserModel>
            {
                new UserModel { Id = 1 },
                new UserModel { Id = 2 },
                new UserModel { Id = 3 },
                new UserModel { Id = 4 },
                new UserModel { Id = 5 },
                new UserModel { Id = 6 },
                new UserModel { Id = 7 },
                new UserModel { Id = 8 },
                new UserModel { Id = 9 },
                new UserModel { Id = 10 },
                new UserModel { Id = 11 },
            };

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetUsers(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int index, int size) => users.Skip(index * size).Take(size));

            var query = new GetUsersQuery { PageIndex = pageIndex, PageSize = pageSize };

            var res = await new GetUsersHandler(repositoryMock.Object).Handle(query, default);

            var testList = users.Skip((pageIndex ?? 0) * (pageSize ?? 10)).Take(pageSize ?? 10);

            Assert.Equivalent(testList.Select(x => x.Id), res.Select(x => x.Id), true);
        }

        [Fact]
        public async Task LoginCommand_ValidPassword_Success()
        {
            var authServiceMock = new Mock<IAuthService>();

            authServiceMock.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            const string Token = "token";
            authServiceMock.Setup(x => x.GenerateTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(Token);

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<LoginResponse>()))
                .Returns((LoginResponse data) => new DataResponseModel<LoginResponse> { Data = data, Success = true });

            var userFactoryMock = new Mock<IUserFactory>();
            userFactoryMock.Setup(x => x.CreateLoginResponse(It.IsAny<User>(), It.IsAny<string>()))
                .Returns((User user, string token) => new LoginResponse { AuthToken = token, UserId = user.Id });

            var userRepositoryMock = new Mock<IUserRepository>();

            var user = new User { Id = 1, PasswordHash = "hash" };
            userRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>()))
                .ReturnsAsync(user);

            var command = new LoginCommand
            {
                Password = "pass",
                Login = "login"
            };

            var res = await new LoginHandler(authServiceMock.Object, userRepositoryMock.Object,
                responseFactoryMock.Object, userFactoryMock.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(user.Id, res.Data.UserId);
            Assert.Equal(Token, res.Data.AuthToken);
        }

        [Fact]
        public async Task LoginCommand_InvalidPassword_Fail()
        {
            var authServiceMock = new Mock<IAuthService>();

            authServiceMock.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            const string Token = "token";

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFailure<LoginResponse>(It.IsAny<string>()))
                .Returns((string error) => new DataResponseModel<LoginResponse> { Error = error, Success = false });

            var userFactoryMock = new Mock<IUserFactory>();

            var userRepositoryMock = new Mock<IUserRepository>();

            var user = new User { Id = 1, PasswordHash = "hash" };
            userRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>()))
                .ReturnsAsync(user);

            var command = new LoginCommand
            {
                Password = "pass",
                Login = "login"
            };

            var res = await new LoginHandler(authServiceMock.Object, userRepositoryMock.Object,
                responseFactoryMock.Object, userFactoryMock.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task LoginCommand_UserNotFound_Fail()
        {
            var authServiceMock = new Mock<IAuthService>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFailure<LoginResponse>(It.IsAny<string>()))
                .Returns((string error) => new DataResponseModel<LoginResponse> { Error = error, Success = false });

            var userFactoryMock = new Mock<IUserFactory>();

            var userRepositoryMock = new Mock<IUserRepository>();

            var user = new User { Id = 1, PasswordHash = "hash" };
            userRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var command = new LoginCommand
            {
                Password = "pass",
                Login = "login"
            };

            var res = await new LoginHandler(authServiceMock.Object, userRepositoryMock.Object,
                responseFactoryMock.Object, userFactoryMock.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task RegisterCommand_Success()
        {
            var users = new List<User>();

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.HashPasswordAsync(It.IsAny<string>()))
                .ReturnsAsync("hashs");

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateCreatedSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { NewEntityId = id, Success = true });

            var userFactoryMock = new Mock<IUserFactory>();
            userFactoryMock.Setup(x => x.Create(It.IsAny<AddUserRequest>(), It.IsAny<string>()))
                .Returns((AddUserRequest request, string hash) => new User { Name = request.Name, PasswordHash = hash });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.InsertUser(It.IsAny<User>()))
                .Callback((User user) => users.Add(user))
                .ReturnsAsync(1);

            userRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var command = new RegisterCommand { Login = "log", Name = "name", Password = "pass" };

            var res = await new RegisterHandler(responseFactoryMock.Object, userRepositoryMock.Object,
                userFactoryMock.Object, authServiceMock.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(users, x => x.Name == command.Name);
        }

        [Fact]
        public async Task RegisterCommand_LoginExists_Fail()
        {
            var users = new List<User>();

            var authServiceMock = new Mock<IAuthService>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Error = msg, Success = false });

            var userFactoryMock = new Mock<IUserFactory>();

            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>()))
                .ReturnsAsync(() => new User());

            var command = new RegisterCommand { Login = "log", Name = "name", Password = "pass" };

            var res = await new RegisterHandler(responseFactoryMock.Object, userRepositoryMock.Object,
                userFactoryMock.Object, authServiceMock.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.DoesNotContain(users, x => x.Login == command.Login);
        }
    }
}
