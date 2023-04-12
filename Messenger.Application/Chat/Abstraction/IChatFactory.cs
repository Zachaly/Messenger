using Messenger.Domain.Entity;
using Messenger.Models.Chat.Request;

namespace Messenger.Application.Abstraction
{
    public interface IChatFactory
    {
        Chat Create(AddChatRequest request);
    }
}
