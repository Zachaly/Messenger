using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessageReaction.Request;

namespace Messenger.Application
{
    public class ReactionFactory : IReactionFactory
    {
        public DirectMessageReaction CreateDirectMessageReaction(AddDirectMessageReactionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
