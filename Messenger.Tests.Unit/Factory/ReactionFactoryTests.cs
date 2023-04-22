using Messenger.Application;
using Messenger.Models.ChatMessageReaction.Request;
using Messenger.Models.DirectMessageReaction.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class ReactionFactoryTests
    {
        private ReactionFactory _reactionFactory;

        public ReactionFactoryTests()
        {
            _reactionFactory = new ReactionFactory();
        }

        [Fact]
        public void CreateDirectMessageReaction_Creates_Proper_Entity()
        {
            var request = new AddDirectMessageReactionRequest
            {
                MessageId = 1,
                Reaction = "a"
            };

            var reaction = _reactionFactory.CreateDirectMessageReaction(request);

            Assert.Equal(request.MessageId, reaction.MessageId);
            Assert.Equal(request.Reaction, reaction.Reaction);
        }

        [Fact]
        public void CreateChatMessageReaction_Creates_Proper_Entity()
        {
            var request = new AddChatMessageReactionRequest
            {
                MessageId = 1,
                UserId = 2,
                Reaction = "a"
            };

            var reaction = _reactionFactory.CreateChatMessageReaction(request);

            Assert.Equal(request.UserId, reaction.MessageId);
            Assert.Equal(request.MessageId, reaction.MessageId);
            Assert.Equal(request.Reaction, reaction.Reaction);
        }
    }
}
