using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatMessage.Request;
using Messenger.Models.Response;
using Moq;
using Xunit.Abstractions;

namespace Messenger.Tests.Unit.Command
{
    public class ChatMessageCommandTests
    {
        [Fact]
        public async Task AddChatMessageCommand_Success()
        {
            var messages = new List<ChatMessage>();

            const long MessageId = 1;
            var chatMessageRepository = new Mock<IChatMessageRepository>();
            chatMessageRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessage>()))
                .Callback((ChatMessage msg) => messages.Add(msg))
                .ReturnsAsync(MessageId);

            chatMessageRepository.Setup(x => x.GetAsync(It.IsAny<GetChatMessageRequest>()))
                .ReturnsAsync(messages.Select(x => new ChatMessageModel { Id = x.Id }));

            var chatMessageFactory = new Mock<IChatMessageFactory>();
            chatMessageFactory.Setup(x => x.Create(It.IsAny<AddChatMessageRequest>()))
                .Returns((AddChatMessageRequest request) => new ChatMessage { ChatId = request.ChatId, SenderId = request.SenderId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateCreatedSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.ChatMessageSend(It.IsAny<ChatMessageModel>(), It.IsAny<long>()));

            var command = new AddChatMessageCommand { ChatId = 2, SenderId = 3 };
            var res = await new AddChatMessageHandler(chatMessageRepository.Object, chatMessageFactory.Object, responseFactory.Object,
                notificationService.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Single(messages);
            Assert.Contains(messages, x => x.SenderId == command.SenderId && x.ChatId == command.ChatId);
        }

        [Fact]
        public async Task AddChatMessageCommand_ExceptionThrown_Fail()
        {
            var messages = new List<ChatMessage>();

            const string Error = "error";
            var chatMessageRepository = new Mock<IChatMessageRepository>();
            chatMessageRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessage>()))
                .Callback((ChatMessage msg) => throw new Exception(Error));

            var chatMessageFactory = new Mock<IChatMessageFactory>();
            chatMessageFactory.Setup(x => x.Create(It.IsAny<AddChatMessageRequest>()))
                .Returns((AddChatMessageRequest request) => new ChatMessage { ChatId = request.ChatId, SenderId = request.SenderId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();

            var command = new AddChatMessageCommand { ChatId = 2, SenderId = 3 };
            var res = await new AddChatMessageHandler(chatMessageRepository.Object, chatMessageFactory.Object, responseFactory.Object,
                notificationService.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Empty(messages);
            Assert.DoesNotContain(messages, x => x.SenderId == command.SenderId && x.ChatId == command.ChatId);
        }

        [Fact]
        public async Task GetChatMessageCountQuery_Success()
        {
            const int Count = 10;

            var repository = new Mock<IChatMessageRepository>();
            repository.Setup(x => x.GetCount(It.IsAny<GetChatMessageRequest>()))
                .ReturnsAsync(Count);

            var query = new GetChatMessageCountQuery();
            var res = await new GetChatMessageCountHandler(repository.Object).Handle(query, default);

            Assert.Equal(Count, res);
        }

        [Fact]
        public async Task GetChatMessageQuery_Success()
        {
            var messages = new List<ChatMessageModel>
            {
                new ChatMessageModel { Id = 1 },
                new ChatMessageModel { Id = 2 },
                new ChatMessageModel { Id = 3 }
            };

            var repository = new Mock<IChatMessageRepository>();
            repository.Setup(x => x.GetAsync(It.IsAny<GetChatMessageRequest>()))
                .ReturnsAsync(messages);

            var query = new GetChatMessageQuery();
            var res = await new GetChatMessageHandler(repository.Object).Handle(query, default);

            Assert.Equivalent(messages, res, true);
        }
    }
}
