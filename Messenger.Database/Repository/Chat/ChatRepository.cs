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
        }

        public Task UpdateAsync(UpdateChatRequest updateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
