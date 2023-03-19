using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Application
{
    public class DirectMessageFactory : IDirectMessageFactory
    {
        public DirectMessage Create(AddDirectMessageRequest request)
            => new DirectMessage
            {
                Content = request.Content,
                Created = DateTime.Now,
                Read = false,
                ReceiverId = request.ReceiverId,
                SenderId = request.SenderId,
            };
    }
}
