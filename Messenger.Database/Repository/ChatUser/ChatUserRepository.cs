using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatUser;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Database.Repository
{
    internal class ChatUserRepository : KeylessRepositoryBase<ChatUser, ChatUserModel, GetChatUserRequest>, IChatUserRepository
    {
        public ChatUserRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
        }

        public Task DeleteAsync(long userId, long chatId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateChatUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
