using Messenger.Application;
using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.Chat;
using Messenger.Models.Chat.Request;
using Messenger.Models.Response;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class ChatCommandTests
    {
        [Fact]
        public async Task AddChatCommand_Success()
        {
            var chats = new List<Chat>();
            var chatUsers = new List<ChatUser>();

            const long ChatId = 21;

            var chatRepository = new Mock<IChatRepository>();
            chatRepository.Setup(x => x.InsertAsync(It.IsAny<Chat>()))
                .Callback((Chat chat) => chats.Add(chat))
                .ReturnsAsync(ChatId);

            var chatFactory = new Mock<IChatFactory>();
            chatFactory.Setup(x => x.Create(It.IsAny<AddChatRequest>()))
                .Returns((AddChatRequest request) => new Chat { CreatorId = request.UserId, Name = request.Name });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateCreatedSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

            var chatUserRepository = new Mock<IChatUserRepository>();
            chatUserRepository.Setup(x => x.InsertAsync(It.IsAny<ChatUser>()))
                .Callback((ChatUser user) => chatUsers.Add(user));

            var chatUserFactory = new Mock<IChatUserFactory>();
            chatUserFactory.Setup(x => x.Create(It.IsAny<long>(), It.IsAny<long>(), true))
                .Returns((long chat, long user, bool admin) => new ChatUser { ChatId = chat, UserId = user });

            var command = new AddChatCommand { Name = "name", UserId = 37 };
            var res = await new AddChatHandler(chatRepository.Object, chatFactory.Object, responseFactory.Object, chatUserRepository.Object,
                chatUserFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Single(chats);
            Assert.Single(chatUsers);
            Assert.Contains(chats, x => x.Name == command.Name && x.CreatorId == command.UserId);
            Assert.Contains(chatUsers, x => x.UserId == command.UserId && x.ChatId == ChatId);
        }

        [Fact]
        public async Task AddChatCommand_ExceptionThrown_Fail()
        {
            var chats = new List<Chat>();
            var chatUsers = new List<ChatUser>();

            const string Error = "Error";
            var chatRepository = new Mock<IChatRepository>();
            chatRepository.Setup(x => x.InsertAsync(It.IsAny<Chat>()))
                .Callback((Chat chat) => throw new Exception(Error));

            var chatFactory = new Mock<IChatFactory>();
            chatFactory.Setup(x => x.Create(It.IsAny<AddChatRequest>()))
                .Returns((AddChatRequest request) => new Chat { CreatorId = request.UserId, Name = request.Name });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var chatUserRepository = new Mock<IChatUserRepository>();

            var chatUserFactory = new Mock<IChatUserFactory>();

            var command = new AddChatCommand { Name = "name", UserId = 37 };
            var res = await new AddChatHandler(chatRepository.Object, chatFactory.Object, responseFactory.Object, chatUserRepository.Object,
                chatUserFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Empty(chats);
            Assert.Empty(chatUsers);
        }

        [Fact]
        public async Task GetChatQuery_Success()
        {
            var chats = new List<ChatModel>
            {
                new ChatModel { Id = 1 },
                new ChatModel { Id = 2 },
                new ChatModel { Id = 3 },
            };
            var chatRepository = new Mock<IChatRepository>();
            chatRepository.Setup(x => x.GetAsync(It.IsAny<GetChatRequest>()))
                .ReturnsAsync(chats);

            var query = new GetChatQuery();
            var res = await new GetChatHandler(chatRepository.Object).Handle(query, default);

            Assert.Equivalent(chats, res, true);
        }

        [Fact]
        public async Task UpdateChatCommand_Success()
        {
            var chat = new Chat { Name = "old name" };

            var chatRepository = new Mock<IChatRepository>();
            chatRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateChatRequest>()))
                .Callback((UpdateChatRequest request) => chat.Name = request.Name);

            chatRepository.Setup(x => x.GetAsync(It.IsAny<GetChatRequest>()))
                .ReturnsAsync(new List<ChatModel> { new ChatModel { Name = chat.Name } });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.ChatUpdated(It.IsAny<ChatModel>()));

            var command = new UpdateChatCommand { Id = 1, Name = "name" };
            var res = await new UpdateChatHandler(chatRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(chat.Name, command.Name);
        }

        [Fact]
        public async Task UpdateChatCommand_ExceptionThrown_Fail()
        {
            var chat = new Chat { Name = "old name" };

            const string Error = "Error";
            var chatRepository = new Mock<IChatRepository>();
            chatRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateChatRequest>()))
                .Callback((UpdateChatRequest request) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();

            var command = new UpdateChatCommand { Id = 1, Name = "name" };
            var res = await new UpdateChatHandler(chatRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
            Assert.NotEqual(chat.Name, command.Name);
        }
    }
}
