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
            DefaultOrderBy = "[Created] DESC";
        }

        public Task<DirectMessageModel> GetByIdAsync(long id)
        {
            var query = _sqlQueryBuilder.Where(new GetDirectMessagesRequest { Id = id }).BuildSelect<DirectMessageModel>(Table);

            return QuerySingleAsync<DirectMessageModel>(query.Query, query.Params);
        }

        public Task UpdateAsync(UpdateDirectMessageRequest request)
        {
            var query = _sqlQueryBuilder.BuildSet(request, Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
