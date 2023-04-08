using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageRead.Request;

namespace Messenger.Application.Abstraction
{
    public interface IChatMessageReadFactory
    {
        ChatMessageRead Create(AddChatMessageReadRequest request);
    }
}
