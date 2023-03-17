using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Application
{
    public class DirectMessageFactory : IDirectMessageFactory
    {
        public DirectMessage Create(AddDirectMessageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
