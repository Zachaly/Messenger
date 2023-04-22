using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageReaction.Request;

namespace Messenger.Database.Repository
{
    public class ChatMessageReactionRepository : KeylessRepositoryBase<ChatMessageReaction, ChatMessageReaction, GetChatMessageReactionRequest>,
        IChatMessageReactionRepository
    {
        public ChatMessageReactionRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "ChatMessageReaction";
            DefaultOrderBy = "MessageId";
        }

        public Task DeleteAsync(long userId, long messageId)
        {
            var query = _sqlQueryBuilder.Where(new { UserId = userId, MessageId = messageId }).BuildDelete(Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
