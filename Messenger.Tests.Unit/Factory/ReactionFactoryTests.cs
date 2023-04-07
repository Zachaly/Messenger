using Messenger.Application;
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
    }
}
