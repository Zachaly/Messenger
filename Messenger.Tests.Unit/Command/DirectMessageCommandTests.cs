using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage;
using Messenger.Models.DirectMessage.Request;
using Messenger.Models.Response;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class DirectMessageCommandTests
    {
        [Fact]
        public async Task AddDirectMessageCommand_Success()
        {
            var messages = new List<DirectMessage>();

            var repository = new Mock<IDirectMessageRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<DirectMessage>()))
                .Callback((DirectMessage message) => messages.Add(message))
                .ReturnsAsync(1);

            repository.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new DirectMessageModel());

            var messageFactory = new Mock<IDirectMessageFactory>();

            messageFactory.Setup(x => x.Create(It.IsAny<AddDirectMessageRequest>()))
                .Returns(new DirectMessage());

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateCreatedSuccess(It.IsAny<long>()))
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();

            var command = new AddDirectMessageCommand { };

            var response = await new AddDirectMessageHandler(responseFactory.Object, messageFactory.Object, repository.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(response.Success);
            Assert.Single(messages);
        }

        [Fact]
        public async Task GetDirectMessagesQuery_Success()
        {
            var data = new List<DirectMessageModel>
            {
                new DirectMessageModel { Id = 1 },
                new DirectMessageModel { Id = 2 },
                new DirectMessageModel { Id = 3 },
            };

            var repository = new Mock<IDirectMessageRepository>();
            repository.Setup(x => x.GetAsync(It.IsAny<GetDirectMessagesRequest>()))
                .ReturnsAsync(data);

            var query = new GetDirectMessagesQuery();

            var response = await new GetDirectMessagesHandler(repository.Object).Handle(query, default);

            Assert.Equivalent(data, response);
        }

        [Fact]
        public async Task UpdateDirectMessageCommand_Success()
        {
            var message = new DirectMessage { Read = false, Id = 1 };

            var repository = new Mock<IDirectMessageRepository>();
            repository.Setup(x => x.UpdateAsync(It.IsAny<UpdateDirectMessageRequest>()))
                .Callback((UpdateDirectMessageRequest request) =>
                {
                    message.Read = request.Read ?? message.Read;
                });

            repository.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new DirectMessageModel { SenderId = message.SenderId, Id = message.Id });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();

            var command = new UpdateDirectMessageCommand { Id = message.Id, Read = true };

            var res = await new UpdateDirectMessageHandler(responseFactory.Object, repository.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(message.Read);
            Assert.True(res.Success);
        }

        [Fact]
        public async Task UpdateDirectMessageCommand_ExceptionThrown_Fail()
        {
            const string Error = "Error";

            var repository = new Mock<IDirectMessageRepository>();
            repository.Setup(x => x.UpdateAsync(It.IsAny<UpdateDirectMessageRequest>()))
                .Callback((UpdateDirectMessageRequest request) =>
                {
                    throw new Exception(Error);
                });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Error = msg, Success = false });

            var notificationService = new Mock<INotificationService>();

            var command = new UpdateDirectMessageCommand { Id = 1, Read = true };

            var res = await new UpdateDirectMessageHandler(responseFactory.Object, repository.Object, notificationService.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task GetDirectMessageCountQuery_Success()
        {
            const int Count = 10;
            var repository = new Mock<IDirectMessageRepository>();
            repository.Setup(x => x.GetCount(It.IsAny<GetDirectMessagesRequest>()))
                .ReturnsAsync(Count);

            var query = new GetDirectMessageCountQuery();

            var res = await new GetDirectMessageCountHandler(repository.Object).Handle(query, default);

            Assert.Equal(Count, res);
        }
    }
}
