using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Database.Repository
{
    public class DirectMessageRepository : RepositoryBase<DirectMessage, DirectMessageModel, GetDirectMessagesRequest>, IDirectMessageRepository
    {
        public DirectMessageRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) 
            : base(connectionFactory, sqlQueryBuilder)
        {
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
            Table = "DirectMessage";
        }

        public Task<DirectMessageModel> GetByAsyncAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateDirectMessageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
