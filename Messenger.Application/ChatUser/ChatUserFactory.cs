using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Application
{
    public class ChatUserFactory : IChatUserFactory
    {
        public ChatUser Create(AddChatUserRequest request)
            => new ChatUser
            {
                IsAdmin = false,
                ChatId = request.ChatId,
                UserId = request.UserId,
            };

        public ChatUser Create(long chatId, long userId, bool isAdmin = true)
            => new ChatUser
            {
                ChatId = chatId,
                IsAdmin = isAdmin,
                UserId = userId,
            };
    }
}
