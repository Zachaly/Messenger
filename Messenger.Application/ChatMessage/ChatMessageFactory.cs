using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Application
{
    public class ChatMessageFactory : IChatMessageFactory
    {
        public ChatMessage Create(AddChatMessageRequest request)
            => new ChatMessage
            {
                ChatId = request.ChatId,
                Content = request.Content,
                Created = DateTime.Now,
                SenderId = request.SenderId,
            };
    }
}
