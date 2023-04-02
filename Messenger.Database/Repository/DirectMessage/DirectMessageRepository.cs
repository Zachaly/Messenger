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

        public override async Task<IEnumerable<DirectMessageModel>> GetAsync(GetDirectMessagesRequest request)
        {
            var query = _sqlQueryBuilder.Join(request)
                .Where(request)
                .OrderBy(DefaultOrderBy)
                .AddPagination(request)
                .BuildSelect<DirectMessageModel>(Table);

            using(var connection = _connectionFactory.GetConnection())
            {
                var lookup = new Dictionary<long, DirectMessageModel>();
                await connection.QueryAsync<DirectMessageModel, DirectMessageImage, DirectMessageModel>(query.Query, (model, image) =>
                {
                    DirectMessageModel message;
                    if(!lookup.TryGetValue(model.Id, out message))
                    {
                        lookup.Add(model.Id, model);
                        message = model;
                    }
                    message.ImageIds ??= new List<long>();
                    if(image is not null)
                    {
                        (message.ImageIds as List<long>)!.Add(image.Id);
                    }
                    
                    return model;
                }, query.Params);
                return lookup.Values;
            }
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
