using Dapper;
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
            Table = "ChatMessage";
            DefaultOrderBy = "[Id] DESC";
        }

        public override async Task<IEnumerable<ChatMessageModel>> GetAsync(GetChatMessageRequest request)
        {
            var query = _sqlQueryBuilder.Join(request)
                .Where(request)
                .AddPagination(request)
                .OrderBy(DefaultOrderBy)
                .BuildSelect<ChatMessageModel>(Table);

            using(var connection = _connectionFactory.GetConnection())
            {
                var lookup = new Dictionary<long, ChatMessageModel>();

                await connection.QueryAsync<ChatMessageModel, ChatMessageRead, ChatMessageModel>(query.Query, (message, read) =>
                {
                    ChatMessageModel msg;

                    if(!lookup.TryGetValue(message.Id, out msg))
                    {
                        lookup.Add(message.Id, message);
                        msg = message;
                    }

                    msg.ReadByIds ??= new List<long>();

                    if(read is not null)
                    {
                        (msg.ReadByIds as List<long>)!.Add(read.UserId);
                    }

                    return message;
                }, query.Params, splitOn: "MessageId");

                return lookup.Values;
            }
        }
    }
}
