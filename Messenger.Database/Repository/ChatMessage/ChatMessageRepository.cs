using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Database.Repository
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage, ChatMessageModel, GetChatMessageRequest>, IChatMessageRepository
    {
        public ChatMessageRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
        }
    }
}
