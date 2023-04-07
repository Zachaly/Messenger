using Messenger.Application.Abstraction;
using Messenger.Application.Reaction.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessageReaction.Request;
using Messenger.Models.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
