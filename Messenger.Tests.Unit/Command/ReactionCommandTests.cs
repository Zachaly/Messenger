using FluentValidation.Results;
using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageReaction.Request;
using Messenger.Models.DirectMessageReaction.Request;
using Messenger.Models.Response;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class ReactionCommandTests
    {
        [Fact]
        public async Task AddDirectMessageReactionCommand_Success()
        {
            var reactions = new List<DirectMessageReaction>();

            var reactionFactory = new Mock<IReactionFactory>();
            reactionFactory.Setup(x => x.CreateDirectMessageReaction(It.IsAny<AddDirectMessageReactionRequest>()))
                .Returns((AddDirectMessageReactionRequest request) => new DirectMessageReaction
                {
                    MessageId = request.MessageId,
                    Reaction = request.Reaction,
                });

            var reactionRepository = new Mock<IDirectMessageReactionRepository>();
            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>()));

            reactionRepository.Setup(x => x.InsertAsync(It.IsAny<DirectMessageReaction>()))
                .Callback((DirectMessageReaction reaction) => reactions.Add(reaction));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.DirectMessageReactionChanged(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long>()));

            var command = new AddDirectMessageReactionCommand { Reaction = "r", MessageId = 1, ReceiverId = 2 };

            var res = await new AddDirectMessageReactionHandler(reactionRepository.Object, notificationService.Object,
                reactionFactory.Object, responseFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(reactions, x => x.MessageId == command.MessageId && x.Reaction == command.Reaction);
        }

        [Fact]
        public async Task AddDirectMessageReactionCommand_ExceptionThrown_Fail()
        {
            var reactions = new List<DirectMessageReaction>();

            var reactionFactory = new Mock<IReactionFactory>();
            reactionFactory.Setup(x => x.CreateDirectMessageReaction(It.IsAny<AddDirectMessageReactionRequest>()))
                .Returns((AddDirectMessageReactionRequest request) => new DirectMessageReaction
                {
                    MessageId = request.MessageId,
                    Reaction = request.Reaction,
                });

            const string Error = "err";

            var reactionRepository = new Mock<IDirectMessageReactionRepository>();
            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>()));

            reactionRepository.Setup(x => x.InsertAsync(It.IsAny<DirectMessageReaction>()))
                .Callback(() => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.DirectMessageReactionChanged(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long>()));

            var command = new AddDirectMessageReactionCommand { Reaction = "r", MessageId = 1, ReceiverId = 2 };

            var res = await new AddDirectMessageReactionHandler(reactionRepository.Object, notificationService.Object,
                reactionFactory.Object, responseFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
            Assert.Empty(reactions);
        }

        [Fact]
        public async Task DeleteDirectMessageReactionCommand_Success()
        {
            const long MessageId = 1;
            var reactions = new List<DirectMessageReaction>
            {
                new DirectMessageReaction { MessageId = MessageId },
                new DirectMessageReaction { MessageId = 2 },
                new DirectMessageReaction { MessageId = 3 },
            };

            var reactionRepository = new Mock<IDirectMessageReactionRepository>();
            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>()))
                .Callback((long id) => reactions.Remove(reactions.First(x => x.MessageId == id)));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.DirectMessageReactionChanged(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long>()));

            var command = new DeleteDirectMessageReactionCommand { MessageId = MessageId, ReceiverId = 2 };

            var res = await new DeleteDirectMessageReactionHandler(notificationService.Object, reactionRepository.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(reactions, x => x.MessageId == command.MessageId);
        }

        [Fact]
        public async Task DeleteDirectMessageReactionCommand_ExceptionThrown_Fail()
        {
            const long MessageId = 1;
            var reactions = new List<DirectMessageReaction>
            {
                new DirectMessageReaction { MessageId = MessageId },
                new DirectMessageReaction { MessageId = 2 },
                new DirectMessageReaction { MessageId = 3 },
            };

            const string Error = "err";

            var reactionRepository = new Mock<IDirectMessageReactionRepository>();
            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>()))
                .Callback((long id) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.DirectMessageReactionChanged(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long>()));

            var command = new DeleteDirectMessageReactionCommand { MessageId = MessageId, ReceiverId = 2 };

            var res = await new DeleteDirectMessageReactionHandler(notificationService.Object, reactionRepository.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Contains(reactions, x => x.MessageId == command.MessageId);
        }

        [Fact]
        public async Task AddChatMessageReactionCommand_Success()
        {
            const long MessageId = 1;
            const long UserId = 2;

            var reactions = new List<ChatMessageReaction>
            {
                new ChatMessageReaction { MessageId = MessageId, UserId = UserId },
                new ChatMessageReaction { MessageId = 3, UserId = 4 },
                new ChatMessageReaction { MessageId = 5, UserId = 6 },
            };

            var reactionRepository = new Mock<IChatMessageReactionRepository>();
            reactionRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessageReaction>()))
                .Callback((ChatMessageReaction reaction) => reactions.Add(reaction));

            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long userId, long messageId) => reactions.Remove(reactions.First(x => x.UserId == userId && x.MessageId == messageId)));

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.ChatMessageReactionChanged(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string?>()));

            var reactionFactory = new Mock<IReactionFactory>();
            reactionFactory.Setup(x => x.CreateChatMessageReaction(It.IsAny<AddChatMessageReactionRequest>()))
                .Returns((AddChatMessageReactionRequest request) => new ChatMessageReaction
                {
                    MessageId = request.MessageId,
                    Reaction = request.Reaction,
                    UserId = request.UserId
                });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new AddChatMessageReactionCommand { ChatId = 0, MessageId = MessageId, Reaction = "a", UserId = UserId };
            var res = await new AddChatMessageReactionHandler(reactionRepository.Object, responseFactory.Object, notificationService.Object,
                reactionFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(reactions, x => x.Reaction == command.Reaction && x.MessageId == command.MessageId && x.UserId == UserId);
            Assert.Single(reactions, x => x.MessageId == command.MessageId && x.UserId == UserId);
        }

        [Fact]
        public async Task AddChatMessageReactionCommand_ExceptionThrown_Fail()
        {
            const long MessageId = 1;
            const long UserId = 2;

            var reactions = new List<ChatMessageReaction>
            {
                new ChatMessageReaction { MessageId = MessageId, UserId = UserId },
                new ChatMessageReaction { MessageId = 3, UserId = 4 },
                new ChatMessageReaction { MessageId = 5, UserId = 6 },
            };

            const string Error = "error";

            var reactionRepository = new Mock<IChatMessageReactionRepository>();
            reactionRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessageReaction>()))
                .Callback((ChatMessageReaction reaction) => throw new Exception(Error));

            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long userId, long messageId) => reactions.Remove(reactions.First(x => x.UserId == userId && x.MessageId == messageId)));

            var reactionFactory = new Mock<IReactionFactory>();
            reactionFactory.Setup(x => x.CreateChatMessageReaction(It.IsAny<AddChatMessageReactionRequest>()))
                .Returns((AddChatMessageReactionRequest request) => new ChatMessageReaction
                {
                    MessageId = request.MessageId,
                    Reaction = request.Reaction,
                    UserId = request.UserId
                });

            var notificationService = new Mock<INotificationService>();

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var command = new AddChatMessageReactionCommand { ChatId = 0, MessageId = MessageId, Reaction = "a", UserId = UserId };
            var res = await new AddChatMessageReactionHandler(reactionRepository.Object, responseFactory.Object, notificationService.Object,
                reactionFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.DoesNotContain(reactions, x => x.Reaction == command.Reaction && x.MessageId == command.MessageId && x.UserId == UserId);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteChatMessageReactionCommand_Success()
        {
            const long MessageId = 1;
            const long UserId = 2;

            var reactions = new List<ChatMessageReaction>
            {
                new ChatMessageReaction { MessageId = MessageId, UserId = UserId },
                new ChatMessageReaction { MessageId = 3, UserId = 4 },
                new ChatMessageReaction { MessageId = 5, UserId = 6 },
            };

            var reactionRepository = new Mock<IChatMessageReactionRepository>();
            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long userId, long messageId) => reactions.Remove(reactions.First(x => x.UserId == userId && x.MessageId == messageId)));

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.ChatMessageReactionChanged(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string?>()));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = false });

            var command = new DeleteChatMessageReactionCommand { ChatId = 0, MessageId = MessageId, UserId = UserId };
            var res = await new DeleteChatMessageReactionHandler(reactionRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(reactions, x => x.UserId == command.UserId && x.MessageId == command.UserId);
        }

        [Fact]
        public async Task DeleteChatMessageReactionCommand_ExceptionThrown_Fail()
        {
            const long MessageId = 1;
            const long UserId = 2;

            var reactions = new List<ChatMessageReaction>
            {
                new ChatMessageReaction { MessageId = MessageId, UserId = UserId },
                new ChatMessageReaction { MessageId = 3, UserId = 4 },
                new ChatMessageReaction { MessageId = 5, UserId = 6 },
            };

            const string Error = "Error";

            var reactionRepository = new Mock<IChatMessageReactionRepository>();
            reactionRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long userId, long messageId) => reactions.Remove(reactions.First(x => x.UserId == userId && x.MessageId == messageId)));

            var notificationService = new Mock<INotificationService>();

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            var command = new DeleteChatMessageReactionCommand { ChatId = 0, MessageId = MessageId, UserId = UserId };
            var res = await new DeleteChatMessageReactionHandler(reactionRepository.Object, responseFactory.Object, notificationService.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Contains(reactions, x => x.UserId == command.UserId && x.MessageId == command.UserId);
            Assert.Equal(Error, res.Error);
        }
    }
}
