using Messenger.Domain.Entity;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Application.Abstraction
{
    public interface IChatUserFactory
    {
        ChatUser Create(AddChatUserRequest request);
    }
}
