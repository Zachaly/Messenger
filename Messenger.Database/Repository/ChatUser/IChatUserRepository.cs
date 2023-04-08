using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatUser;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Database.Repository
{
    public interface IChatUserRepository : IKeylessRepository<ChatUser, ChatUserModel, GetChatUserRequest>
    {
        Task DeleteAsync(long userId, long chatId);
    }
}
