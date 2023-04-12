using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageRead.Request;

namespace Messenger.Database.Repository
{
    public class ChatMessageReadRepository : KeylessRepositoryBase<ChatMessageRead, ChatMessageRead, GetChatMessageReadRequest>,
        IChatMessageReadRepository
    {
        public ChatMessageReadRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
        }
    }
}
