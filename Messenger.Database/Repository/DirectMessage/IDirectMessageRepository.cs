using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Database.Repository
{
    public interface IDirectMessageRepository : IRepository<DirectMessage, DirectMessageModel, GetDirectMessagesRequest>
    {
        Task<DirectMessageModel> GetByAsyncAsync(long id);
        Task UpdateAsync(UpdateDirectMessageRequest request);
    }
}
