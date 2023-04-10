using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatUser;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Database.Repository
{
    public class ChatUserRepository : KeylessRepositoryBase<ChatUser, ChatUserModel, GetChatUserRequest>, IChatUserRepository
    {
        public ChatUserRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "ChatUser";
            DefaultOrderBy = "[ChatId]";
        }

        public Task DeleteAsync(long userId, long chatId)
        {
            var query = _sqlQueryBuilder.Where(new { UserId = userId, ChatId = chatId }).BuildDelete(Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }

        public Task UpdateAsync(UpdateChatUserRequest request)
        {
            var query = _sqlQueryBuilder.Where(request).BuildSet(request, Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
