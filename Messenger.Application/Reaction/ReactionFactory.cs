using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageReaction.Request;
using Messenger.Models.DirectMessageReaction.Request;

namespace Messenger.Application
{
    public class ReactionFactory : IReactionFactory
    {
        public ChatMessageReaction CreateChatMessageReaction(AddChatMessageReactionRequest request)
        {
            throw new NotImplementedException();
        }

        public DirectMessageReaction CreateDirectMessageReaction(AddDirectMessageReactionRequest request)
            => new DirectMessageReaction
            {
                MessageId = request.MessageId,
                Reaction = request.Reaction,
            };
    }
}
