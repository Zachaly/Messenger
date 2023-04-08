using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Application.Abstraction
{
    public interface IChatMessageFactory
    {
        ChatMessage Create(AddChatMessageRequest request);
    }
}
