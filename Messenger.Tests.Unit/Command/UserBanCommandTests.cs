using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.Response;
using Messenger.Models.UserBan;
using Messenger.Models.UserBan.Request;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Command
{
    public class UserBanCommandTests
    {

        [Fact]
        public async Task GetUserBanQuery_Success()
        {
            var bans = new List<UserBanModel>
            {
                new UserBanModel { Id = 1, },
                new UserBanModel { Id = 2, },
                new UserBanModel { Id = 3, },
            };
            var repository = new Mock<IUserBanRepository>();
            repository.Setup(x => x.GetAsync(It.IsAny<GetUserBanRequest>()))
                .ReturnsAsync(bans);

            var query = new GetUserBanQuery();
            var res = await new GetUserBanHandler(repository.Object).Handle(query, default);

            Assert.Equivalent(bans.Select(x => x.Id), res.Select(x => x.Id));
        }

        [Fact]
        public async Task AddUserBanCommand_Success()
        {
            var bans = new List<UserBan>();

            var factory = new Mock<IUserBanFactory>();
            factory.Setup(x => x.Create(It.IsAny<AddUserBanRequest>()))
                .Returns((AddUserBanRequest request) => new UserBan { UserId = request.UserId });

            var repository = new Mock<IUserBanRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<UserBan>()))
                .Callback((UserBan ban) => bans.Add(ban));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.Banned(It.IsAny<long>(), It.IsAny<DateTime>()));

            var command = new AddUserBanCommand { UserId = 1 };
            var res = await new AddUserBanHandler(factory.Object, repository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(bans, x => x.UserId == command.UserId);
        }

        [Fact]
        public async Task AddUserBanCommand_ExceptionThrown_Failure()
        {
            var bans = new List<UserBan>();

            var factory = new Mock<IUserBanFactory>();
            factory.Setup(x => x.Create(It.IsAny<AddUserBanRequest>()))
                .Returns((AddUserBanRequest request) => new UserBan { UserId = request.UserId });

            const string Error = "err";

            var repository = new Mock<IUserBanRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<UserBan>()))
                .Callback((UserBan ban) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.Banned(It.IsAny<long>(), It.IsAny<DateTime>()));

            var command = new AddUserBanCommand { UserId = 1 };
            var res = await new AddUserBanHandler(factory.Object, repository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
            Assert.Empty(bans);
        }

        [Fact]
        public async Task DeleteUserBanCommand_Success()
        {
            const long Id = 2;

            var bans = new List<UserBan>
            {
                new UserBan { Id = 1 },
                new UserBan { Id = Id },
                new UserBan { Id = 3 },
                new UserBan { Id = 4 },
            };

            var repository = new Mock<IUserBanRepository>();

            repository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => bans.Remove(bans.First(x => x.Id == id)));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new DeleteUserBanCommand { Id = Id };
            var res = await new DeleteUserBanHandler(repository.Object, responseFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(bans, x => x.Id == command.Id);
        }

        [Fact]
        public async Task DeleteUserBanCommand_ExceptionThrown_Fail()
        {
            const long Id = 2;

            var bans = new List<UserBan>
            {
                new UserBan { Id = 1 },
                new UserBan { Id = Id },
                new UserBan { Id = 3 },
                new UserBan { Id = 4 },
            };

            const string Error = "err";

            var repository = new Mock<IUserBanRepository>();
            repository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var command = new DeleteUserBanCommand { Id = Id };
            var res = await new DeleteUserBanHandler(repository.Object, responseFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
            Assert.Contains(bans, x => x.Id == command.Id);
        }
    }
}
