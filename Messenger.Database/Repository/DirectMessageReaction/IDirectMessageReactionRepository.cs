using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessageReaction.Request;

namespace Messenger.Database.Repository
{
    public interface IDirectMessageReactionRepository 
        : IKeylessRepository<DirectMessageReaction, DirectMessageReaction, UpdateDirectMessageReactionRequest>
    {
        Task DeleteAsync(long messageId);
    }
}
