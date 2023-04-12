using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Database.Repository
{
    public interface IChatMessageRepository : IRepository<ChatMessage, ChatMessageModel, GetChatMessageRequest>
    {
    }
}
