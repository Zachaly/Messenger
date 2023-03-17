using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Application.Abstraction
{
    public interface IDirectMessageFactory
    {
        DirectMessage Create(AddDirectMessageRequest request);
    }
}
