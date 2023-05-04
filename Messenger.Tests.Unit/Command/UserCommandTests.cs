using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.Response;
using Messenger.Models.User;
using Messenger.Models.User.Request;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;
using Moq;
using System.Security.Claims;

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
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<GetUserRequest>()))
                .ReturnsAsync((GetUserRequest request) => users.Skip((request.PageIndex ?? 0) * (request.PageSize ?? 10)).Take(request.PageSize ?? 10));

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
            authServiceMock.Setup(x => x.GenerateTokenAsync(It.IsAny<User>(), It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(Token);

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<LoginResponse>()))
                .Returns((LoginResponse data) => new DataResponseModel<LoginResponse> { Data = data, Success = true });

            var userFactoryMock = new Mock<IUserFactory>();
            userFactoryMock.Setup(x => x.CreateLoginResponse(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<IEnumerable<UserClaimModel>>()))
                .Returns((User user, string token, IEnumerable<UserClaimModel> _) => new LoginResponse { AuthToken = token, UserId = user.Id });

            var userRepositoryMock = new Mock<IUserRepository>();

            var user = new User { Id = 1, PasswordHash = "hash" };
            userRepositoryMock.Setup(x => x.GetByLoginAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var userClaimRepositoryMock = new Mock<IUserClaimRepository>();
            userClaimRepositoryMock.Setup(x => x.GetAsync(It.IsAny<GetUserClaimRequest>()))
                .ReturnsAsync(new List<UserClaimModel>());

            var userClaimFactoryMock = new Mock<IUserClaimFactory>();
            userClaimFactoryMock.Setup(x => x.CreateSystemClaimFromModel(It.IsAny<UserClaimModel>()))
                .Returns(new Claim("", ""));

            var command = new LoginCommand
            {
                Password = "pass",
                Login = "login"
            };


            var res = await new LoginHandler(authServiceMock.Object, userRepositoryMock.Object,
                responseFactoryMock.Object, userFactoryMock.Object, userClaimRepositoryMock.Object, userClaimFactoryMock.Object)
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
            userRepositoryMock.Setup(x => x.GetByLoginAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var userClaimRepositoryMock = new Mock<IUserClaimRepository>();
            
            var userClaimFactoryMock = new Mock<IUserClaimFactory>();

            var command = new LoginCommand
            {
                Password = "pass",
                Login = "login"
            };

            var res = await new LoginHandler(authServiceMock.Object, userRepositoryMock.Object,
                responseFactoryMock.Object, userFactoryMock.Object, userClaimRepositoryMock.Object, userClaimFactoryMock.Object)
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
            userRepositoryMock.Setup(x => x.GetByLoginAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var userClaimRepositoryMock = new Mock<IUserClaimRepository>();

            var userClaimFactoryMock = new Mock<IUserClaimFactory>();

            var command = new LoginCommand
            {
                Password = "pass",
                Login = "login"
            };

            var res = await new LoginHandler(authServiceMock.Object, userRepositoryMock.Object,
                responseFactoryMock.Object, userFactoryMock.Object, userClaimRepositoryMock.Object, userClaimFactoryMock.Object)
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
            userRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<User>()))
                .Callback((User user) => users.Add(user))
                .ReturnsAsync(1);

            userRepositoryMock.Setup(x => x.GetByLoginAsync(It.IsAny<string>()))
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

            userRepositoryMock.Setup(x => x.GetByLoginAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new User());

            var command = new RegisterCommand { Login = "log", Name = "name", Password = "pass" };

            var res = await new RegisterHandler(responseFactoryMock.Object, userRepositoryMock.Object,
                userFactoryMock.Object, authServiceMock.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.DoesNotContain(users, x => x.Login == command.Login);
        }

        [Fact]
        public async Task GetUserByIdCommand_Success()
        {
            var user = new UserModel { Id = 1, Name = "name" };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            var query = new GetUserByIdQuery { UserId = user.Id };

            var res = await new GetUserByIdHandler(userRepository.Object).Handle(query, default);

            Assert.Equal(user.Id, res.Id);
            Assert.Equal(user.Name, res.Name);
        }

        [Fact]
        public async Task UpdateUsernameCommand_Succcess()
        {
            var user = new User { Id = 1, Name = "name" };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>())).Callback((UpdateUserRequest request) =>
            {
                user.Name = request.Name;
            });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(() => new ResponseModel { Success = true });

            var command = new UpdateUsernameCommand { Id = 1, Name = "new name" };

            var res = await new UpdateUsernameHandler(userRepository.Object, responseFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(user.Name, command.Name);
        }

        [Fact]
        public async Task UpdateUsernameCommand_ExceptionThrown_Failure()
        {
            var user = new User { Id = 1, Name = "name" };

            const string Error = "Error";

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>())).Callback((UpdateUserRequest request) =>
            {
                throw new Exception(Error);
            });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var command = new UpdateUsernameCommand { Id = 1, Name = "new name" };

            var res = await new UpdateUsernameHandler(userRepository.Object, responseFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
            Assert.NotEqual(user.Name, command.Name);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_Success()
        {
            var user = new User { Id = 1, PasswordHash = "hash" };

            var repository = new Mock<IUserRepository>();
            repository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            repository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) =>
                {
                    user.PasswordHash = request.PasswordHash;
                });

            var responseFactory = new Mock<IResponseFactory>();

            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            const string NewHash = "newhash";
            var authService = new Mock<IAuthService>();
            authService.Setup(x => x.HashPasswordAsync(It.IsAny<string>()))
                .ReturnsAsync(NewHash);

            authService.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var command = new ChangeUserPasswordCommand { CurrentPassword = "pass", NewPassword = "new pass" };
            var res = await new ChangeUserPasswordHandler(repository.Object, authService.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(NewHash, user.PasswordHash);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_InvalidPassword_Fail()
        {
            var user = new User { Id = 1, PasswordHash = "hash" };

            var repository = new Mock<IUserRepository>();
            repository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            var responseFactory = new Mock<IResponseFactory>();

            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var authService = new Mock<IAuthService>();

            authService.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var command = new ChangeUserPasswordCommand { CurrentPassword = "pass", NewPassword = "new pass" };
            var res = await new ChangeUserPasswordHandler(repository.Object, authService.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.False(res.Success);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_UserNotFound_Fail()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            var responseFactory = new Mock<IResponseFactory>();

            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var authService = new Mock<IAuthService>();

            var command = new ChangeUserPasswordCommand { CurrentPassword = "pass", NewPassword = "new pass" };
            var res = await new ChangeUserPasswordHandler(repository.Object, authService.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.False(res.Success);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_ExceptionThrown_Fail()
        {
            const string Error = "err";
            var repository = new Mock<IUserRepository>();
            repository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();

            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var authService = new Mock<IAuthService>();

            var command = new ChangeUserPasswordCommand { CurrentPassword = "pass", NewPassword = "new pass" };
            var res = await new ChangeUserPasswordHandler(repository.Object, authService.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
