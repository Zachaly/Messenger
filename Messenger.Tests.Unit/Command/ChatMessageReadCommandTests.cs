using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageRead.Request;
using Messenger.Models.Response;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class ChatMessageReadCommandTests
    {
        [Fact]
        public async Task AddChatMessageReadCommand_Success()
        {
            var messages = new List<ChatMessageRead>();

            var chatMessageReadRepository = new Mock<IChatMessageReadRepository>();
            chatMessageReadRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessageRead>()))
                .Callback((ChatMessageRead msg) => messages.Add(msg));

            var chatMessageReadFactory = new Mock<IChatMessageReadFactory>();
            chatMessageReadFactory.Setup(x => x.Create(It.IsAny<AddChatMessageReadRequest>()))
                .Returns((AddChatMessageReadRequest request) => new ChatMessageRead { MessageId = request.MessageId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.ChatMessageRead(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>()));

            var command = new AddChatMessageReadCommand { MessageId = 1 };
            var res = await new AddChatMessageReadHandler(chatMessageReadRepository.Object, chatMessageReadFactory.Object,
                responseFactory.Object, notificationService.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(messages, x => x.MessageId == command.MessageId);
            Assert.Single(messages);
        }

        [Fact]
        public async Task AddChatMessageReadCommand_ExceptionThrown_Fail()
        {
            var messages = new List<ChatMessageRead>();

            const string Error = "error";
            var chatMessageReadRepository = new Mock<IChatMessageReadRepository>();
            chatMessageReadRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessageRead>()))
                .Callback((ChatMessageRead msg) => throw new Exception(Error));

            var chatMessageReadFactory = new Mock<IChatMessageReadFactory>();
            chatMessageReadFactory.Setup(x => x.Create(It.IsAny<AddChatMessageReadRequest>()))
                .Returns((AddChatMessageReadRequest request) => new ChatMessageRead { MessageId = request.MessageId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();

            var command = new AddChatMessageReadCommand { MessageId = 1 };
            var res = await new AddChatMessageReadHandler(chatMessageReadRepository.Object, chatMessageReadFactory.Object,
                responseFactory.Object, notificationService.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Empty(messages);
            Assert.Equal(Error, res.Error);
        }
    }
}
