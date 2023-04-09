using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Application
{
    public class ChatUserFactory : IChatUserFactory
    {
        public ChatUser Create(AddChatUserRequest request)
        {
            throw new NotImplementedException();
        }

        public ChatUser Create(long chatId, long userId, bool isAdmin = true)
        {
            throw new NotImplementedException();
        }
    }
}
