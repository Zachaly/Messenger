using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public class FriendRequestRepository : RepositoryBase<FriendRequest, FriendRequestModel, GetFriendsRequestsRequest>, IFriendRequestRepository
    {
        public FriendRequestRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory)
            :base(connectionFactory, sqlQueryBuilder)
        {
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
            Table = "FriendRequest";
        }

        public async Task<FriendRequest> GetByIdAsync(long id)
        {
            var query = _sqlQueryBuilder.Where(new { Id = id }).BuildSelect<FriendRequest>(Table);

            using(var conn = _connectionFactory.GetConnection())
            {
                return await conn.QuerySingleOrDefaultAsync<FriendRequest>(query.Query, query.Params);
            }
        }
    }
}
