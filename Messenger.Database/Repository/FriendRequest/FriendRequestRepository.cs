using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly ISqlQueryBuilder _queryBuilder;
        private readonly IConnectionFactory _connectionFactory;

        public FriendRequestRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory)
        {
            _queryBuilder = sqlQueryBuilder;
            _connectionFactory = connectionFactory;
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
        }

        public async Task<IEnumerable<FriendRequestModel>> GetFriendRequests(GetFriendsRequestsRequest request)
        {
            var query = _queryBuilder.Where(request).Join(request).OrderBy("[Id]").BuildSelect<FriendRequestModel>("FriendRequest");

            using(var connection = _connectionFactory.GetConnection())
            {
                return await connection.QueryAsync<FriendRequestModel>(query.Query, query.Params);
            }
        }

        public async Task<FriendRequest> GetRequestById(long id)
        {
            var query = _queryBuilder.Where(new { Id = id }).BuildSelect<FriendRequest>("FriendRequest");

            using(var conn = _connectionFactory.GetConnection())
            {
                return await conn.QuerySingleOrDefaultAsync<FriendRequest>(query.Query, query.Params);
            }
        }

        public async Task<long> InsertFriendRequest(FriendRequest request)
        {
            var query = _queryBuilder.BuildInsert(request);

            using(var connection = _connectionFactory.GetConnection())
            {
                return await connection.QuerySingleAsync<long>(query.Query, query.Params);
            }
        }
    }
}
