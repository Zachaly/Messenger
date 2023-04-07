using Messenger.Domain.Entity;
using Messenger.Models.DirectMessageReaction.Request;

namespace Messenger.Application.Abstraction
{
    public interface IReactionFactory
    {
        DirectMessageReaction CreateDirectMessageReaction(AddDirectMessageReactionRequest request);
    }
}
