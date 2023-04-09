using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.Chat;
using Messenger.Models.ChatUser;
using Messenger.Models.ChatUser.Request;
using Messenger.Models.Response;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class ChatUserCommandTests
    {
        [Fact]
        public async Task GetChatUserQuery_Success()
        {
            var users = new List<ChatUserModel>
            {
                new ChatUserModel { Id = 1 },
                new ChatUserModel { Id = 2 },
                new ChatUserModel { Id = 3 },
            };

            var repository = new Mock<IChatUserRepository>();
            repository.Setup(x => x.GetAsync(It.IsAny<GetChatUserRequest>()))
                .ReturnsAsync(users);

            var query = new GetChatUserQuery();
            var res = await new GetChatUserHandler(repository.Object).Handle(query, default);

            Assert.Equivalent(users, res, true);
        }

        [Fact]
        public async Task GetChatUserCountQuery_Success()
        {
            const int Count = 10;

            var repository = new Mock<IChatUserRepository>();
            repository.Setup(x => x.GetCount(It.IsAny<GetChatUserRequest>()))
                .ReturnsAsync(Count);

            var query = new GetChatUserCountQuery();
            var res = await new GetChatUserCountHandler(repository.Object).Handle(query, default);

            Assert.Equal(Count, res);
        }

        [Fact]
        public async Task AddChatUserCommand_Success()
        {
            var users = new List<ChatUser>();
            
            var chatUserRepository = new Mock<IChatUserRepository>();
            chatUserRepository.Setup(x => x.InsertAsync(It.IsAny<ChatUser>()))
                .Callback((ChatUser user) => users.Add(user));

            chatUserRepository.Setup(x => x.GetAsync(It.IsAny<GetChatUserRequest>()))
                .ReturnsAsync(users.Select(x => new ChatUserModel { Id = x.UserId }));

            var chatUserFactory = new Mock<IChatUserFactory>();
            chatUserFactory.Setup(x => x.Create(It.IsAny<AddChatUserRequest>()))
                .Returns((AddChatUserRequest request) => new ChatUser { ChatId = request.ChatId, UserId = request.UserId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.AddedToChat(It.IsAny<ChatUserModel>(), It.IsAny<long>()));

            var command = new AddChatUserCommand { ChatId = 1, UserId = 2 };
            var res = await new AddChatUserHandler(chatUserRepository.Object, chatUserFactory.Object, responseFactory.Object,
                notificationService.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Single(users);
            Assert.Contains(users, x => x.ChatId == command.ChatId && x.UserId == command.UserId);
        }

        [Fact]
        public async Task AddChatUserCommand_ExceptionThrown_Fail()
        {
            var users = new List<ChatUser>();

            const string Error = "error";
            var chatUserRepository = new Mock<IChatUserRepository>();
            chatUserRepository.Setup(x => x.InsertAsync(It.IsAny<ChatUser>()))
                .Callback((ChatUser user) => throw new Exception(Error));

            var chatUserFactory = new Mock<IChatUserFactory>();
            chatUserFactory.Setup(x => x.Create(It.IsAny<AddChatUserRequest>()))
                .Returns((AddChatUserRequest request) => new ChatUser { ChatId = request.ChatId, UserId = request.UserId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();

            var command = new AddChatUserCommand { ChatId = 1, UserId = 2 };
            var res = await new AddChatUserHandler(chatUserRepository.Object, chatUserFactory.Object, responseFactory.Object,
                notificationService.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Empty(users);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteChatUserCommand_Success()
        {
            var users = new List<ChatUser>
            {
                new ChatUser { ChatId = 1, UserId = 2 },
                new ChatUser { ChatId = 2, UserId = 3 },
                new ChatUser { ChatId = 3, UserId = 4 },
            };

            var chatUserRepository = new Mock<IChatUserRepository>();
            chatUserRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long user, long chat) => users.Remove(users.First(x => x.UserId == user && x.ChatId == chat)));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.RemovedFromChat(It.IsAny<long>(), It.IsAny<long>()));

            var command = new DeleteChatUserCommand { ChatId = 2, UserId = 3 };

            var res = await new DeleteChatUserHandler(chatUserRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(users, x => x.UserId == command.UserId && x.ChatId == command.ChatId);
        }

        [Fact]
        public async Task DeleteChatUserCommand_ExceptionThrown_Fail()
        {
            var users = new List<ChatUser>
            {
                new ChatUser { ChatId = 1, UserId = 2 },
                new ChatUser { ChatId = 2, UserId = 3 },
                new ChatUser { ChatId = 3, UserId = 4 },
            };

            const string Error = "err";
            var chatUserRepository = new Mock<IChatUserRepository>();
            chatUserRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long user, long chat) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();

            var command = new DeleteChatUserCommand { ChatId = 2, UserId = 3 };

            var res = await new DeleteChatUserHandler(chatUserRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Contains(users, x => x.UserId == command.UserId && x.ChatId == command.ChatId);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task UpdateChatUserCommand_Success()
        {
            var user = new ChatUser { ChatId = 1, UserId = 2, IsAdmin = true };

            var chatUserRepository = new Mock<IChatUserRepository>();
            chatUserRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateChatUserRequest>()))
                .Callback((UpdateChatUserRequest request) => user.IsAdmin = request.IsAdmin.GetValueOrDefault());

            chatUserRepository.Setup(x => x.GetAsync(It.IsAny<GetChatUserRequest>()))
                .ReturnsAsync(new List<ChatUserModel> { new ChatUserModel { Id = user.UserId, IsAdmin = user.IsAdmin } });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.ChatUserUpdated(It.IsAny<ChatUserModel>(), It.IsAny<long>()));

            var command = new UpdateChatUserCommand { ChatId = user.ChatId, UserId = user.UserId, IsAdmin = false };
            var res = await new UpdateChatUserHandler(chatUserRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(user.IsAdmin, command.IsAdmin);
        }

        [Fact]
        public async Task UpdateChatUserCommand_ExceptionThrown_Fail()
        {
            var user = new ChatUser { ChatId = 1, UserId = 2, IsAdmin = true };

            const string Error = "err";
            var chatUserRepository = new Mock<IChatUserRepository>();
            chatUserRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateChatUserRequest>()))
                .Callback((UpdateChatUserRequest request) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();

            var command = new UpdateChatUserCommand { ChatId = user.ChatId, UserId = user.UserId, IsAdmin = false };
            var res = await new UpdateChatUserHandler(chatUserRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEqual(user.IsAdmin, command.IsAdmin);
        }
    }
}
