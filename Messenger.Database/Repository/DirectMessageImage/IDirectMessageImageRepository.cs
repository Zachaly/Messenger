using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessageImage.Request;

namespace Messenger.Database.Repository
{
    public interface IDirectMessageImageRepository : IRepository<DirectMessageImage, DirectMessageImage, GetDirectMessageImageRequest>
    {
    }
}
