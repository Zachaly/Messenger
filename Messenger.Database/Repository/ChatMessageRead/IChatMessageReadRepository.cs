using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageRead.Request;

namespace Messenger.Database.Repository
{
    public interface IChatMessageReadRepository : IKeylessRepository<ChatMessageRead, ChatMessageRead, GetChatMessageReadRequest>
    {
    }
}
