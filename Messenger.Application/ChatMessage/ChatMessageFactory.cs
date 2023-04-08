using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Application
{
    public class ChatMessageFactory : IChatMessageFactory
    {
        public ChatMessage Create(AddChatMessageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
