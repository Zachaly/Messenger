using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Models.User;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Messenger.Tests.Unit.Command
{
    public class AuthCommandTests
    {
        [Fact]
        public async Task GetCurrentUserQuery_Success()
        {
            var user = new UserModel { Id = 1, Name = "Test" };

            const string Token = "token";

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.User.Claims)
                .Returns(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) });

            httpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"])
                .Returns($"Bearer {Token}");

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            var userFactory = new Mock<IUserFactory>();
            userFactory.Setup(x => x.CreateLoginResponse(It.IsAny<UserModel>(), It.IsAny<string>()))
                .Returns((UserModel user, string token) => new LoginResponse
                {
                    AuthToken = token,
                    UserId = user.Id,
                    UserName = user.Name,
                });

            var query = new GetCurrentUserQuery();

            var res = await new GetCurrentUserHandler(httpContextAccessor.Object, userFactory.Object, userRepository.Object)
                .Handle(query, default);

            Assert.Equal(user.Name, res.UserName);
            Assert.Equal(user.Id, res.UserId);
            Assert.Equal(Token, res.AuthToken);
        }
    }
}
