using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Chat;
using Messenger.Models.Chat.Request;

namespace Messenger.Database.Repository
{
    public class ChatRepository : RepositoryBase<Chat, ChatModel, GetChatRequest>, IChatRepository
    {
        public ChatRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "Chat";
        }

        public Task UpdateAsync(UpdateChatRequest updateRequest)
        {
            var query = _sqlQueryBuilder.Where(new { Id = updateRequest.Id }).BuildSet(updateRequest, Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
