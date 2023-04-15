using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageImage.Request;

namespace Messenger.Database.Repository
{
    public interface IChatMessageImageRepository : IRepository<ChatMessageImage, ChatMessageImage, GetChatMessageImageRequest>
    {

    }
}
