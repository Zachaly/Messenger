using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.Chat;
using Messenger.Models.Chat.Request;

namespace Messenger.Database.Repository
{
    public interface IChatRepository : IRepository<Chat, ChatModel, GetChatRequest>
    {
        Task DeleteByIdAsync(long id);
        Task UpdateAsync(UpdateChatRequest updateRequest);
    }
}
