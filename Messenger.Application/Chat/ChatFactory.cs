using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.Chat.Request;

namespace Messenger.Application
{
    public class ChatFactory : IChatFactory
    {
        public Chat Create(AddChatRequest request)
            => new Chat
            {
                CreatorId = request.UserId,
                Name = request.Name,
            };
    }
}
