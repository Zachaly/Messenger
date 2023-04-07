using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessageReaction.Request;

namespace Messenger.Database.Repository
{
    public class DirectMessageReactionRepository
        : KeylessRepositoryBase<DirectMessageReaction, DirectMessageReaction, GetDirectMessageReactionRequest>, IDirectMessageReactionRepository
    {
        public DirectMessageReactionRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "DirectMessageReaction";
        }

        public Task DeleteAsync(long messageId)
        {
            var query = _sqlQueryBuilder.Where(new { MessageId = messageId }).BuildDelete(Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
