using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageReaction.Request;

namespace Messenger.Database.Repository
{
    public interface IChatMessageReactionRepository
        :IKeylessRepository<ChatMessageReaction, ChatMessageReaction, GetChatMessageReactionRequest>
    {
        Task DeleteAsync(long userId, long messageId);
    }
}
