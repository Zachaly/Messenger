using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.Response;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class ClaimCommandTests
    {
        [Fact]
        public async Task GetUserClaimQuery_Success()
        {
            var claims = new List<UserClaimModel>
            {
                new UserClaimModel { Value = "val 1"},
                new UserClaimModel { Value = "val 2"}
            };

            var claimRepository = new Mock<IUserClaimRepository>();
            claimRepository.Setup(x => x.GetAsync(It.IsAny<GetUserClaimRequest>()))
                .ReturnsAsync(claims);

            var query = new GetUserClaimQuery();
            var res = await new GetUserClaimHandler(claimRepository.Object).Handle(query, default);

            Assert.Equal(claims, res);
            Assert.Equivalent(claims, res);
        }

        [Fact]
        public async Task AddUserClaimCommand_Success()
        {
            var claims = new List<UserClaim>();

            var claimRepository = new Mock<IUserClaimRepository>();
            claimRepository.Setup(x => x.InsertAsync(It.IsAny<UserClaim>()))
                .Callback((UserClaim claim) => claims.Add(claim));

            var claimFactory = new Mock<IUserClaimFactory>();
            claimFactory.Setup(x => x.Create(It.IsAny<AddUserClaimRequest>()))
                .Returns((AddUserClaimRequest request) => new UserClaim { UserId = request.UserId, Value = request.Value });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.ClaimAdded(It.IsAny<long>(), It.IsAny<string>()));

            var command = new AddUserClaimCommand { UserId = 1, Value = "val" };
            var res = await new AddUserClaimHandler(claimFactory.Object, claimRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(claims, x => x.Value == command.Value && x.UserId == command.UserId);
        }

        [Fact]
        public async Task AddUserClaimCommand_ExceptionThrown_Failure()
        {
            var claims = new List<UserClaim>();

            const string Error = "err";
            var claimRepository = new Mock<IUserClaimRepository>();
            claimRepository.Setup(x => x.InsertAsync(It.IsAny<UserClaim>()))
                .Callback((UserClaim claim) => throw new Exception(Error));

            var claimFactory = new Mock<IUserClaimFactory>();
            claimFactory.Setup(x => x.Create(It.IsAny<AddUserClaimRequest>()))
                .Returns((AddUserClaimRequest request) => new UserClaim { UserId = request.UserId, Value = request.Value });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();

            var command = new AddUserClaimCommand { UserId = 1, Value = "val" };
            var res = await new AddUserClaimHandler(claimFactory.Object, claimRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.DoesNotContain(claims, x => x.Value == command.Value && x.UserId == command.UserId);
        }

        [Fact]
        public async Task DeleteUserClaimCommand_Success()
        {
            var deletedClaim = new UserClaim { UserId = 2, Value = "val2" };

            var claims = new List<UserClaim>
            {
                new UserClaim { UserId = 1, Value = "val" },
                deletedClaim,
                new UserClaim { UserId = 2, Value = "val 3"}
            };

            var claimRepository = new Mock<IUserClaimRepository>();
            claimRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Callback((long id, string claim) => claims.Remove(claims.First(x => x.UserId == id && x.Value == claim)));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new DeleteUserClaimCommand { Claim = deletedClaim.Value, UserId = deletedClaim.UserId };
            var res = await new DeleteUserClamHandler(claimRepository.Object, responseFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(claims, x => x == deletedClaim);
        }

        [Fact]
        public async Task DeleteUserClaimCommand_ExceptionThrown_Failure()
        {
            var deletedClaim = new UserClaim { UserId = 2, Value = "val2" };

            var claims = new List<UserClaim>
            {
                new UserClaim { UserId = 1, Value = "val" },
                deletedClaim,
                new UserClaim { UserId = 2, Value = "val 3"}
            };

            const string Error = "err";

            var claimRepository = new Mock<IUserClaimRepository>();
            claimRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Callback((long id, string claim) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var command = new DeleteUserClaimCommand { Claim = deletedClaim.Value, UserId = deletedClaim.UserId };
            var res = await new DeleteUserClamHandler(claimRepository.Object, responseFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Contains(claims, x => x == deletedClaim);
        }
    }
}
