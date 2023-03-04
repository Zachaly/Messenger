using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;
using Messenger.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Database.Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly ISqlQueryBuilder _queryBuilder;
        private readonly IConnectionFactory _connectionFactory;

        public FriendRepository(ISqlQueryBuilder queryBuilder, IConnectionFactory connectionFactory)
        {
            _queryBuilder = queryBuilder;
            _connectionFactory = connectionFactory;
        }


        public async Task<IEnumerable<FriendListItem>> GetAllFriendsAsync(GetFriendsRequest request)
        {
            var query = _queryBuilder
                .Where(request)
                .BuildSelect<FriendListItem>("Friend");

            using(var connection = _connectionFactory.GetConnection())
            {
                return await connection.QueryAsync<FriendListItem>(query.Query, query.Params);
            }
        }

        public async Task InsertFriendAsync(Friend friend)
        {
            var query = _queryBuilder.BuildInsert(friend, false);

            using(var connection = _connectionFactory.GetConnection())
            {
                await connection.QueryAsync(query.Query, query.Params);
            }
        }
    }
}
