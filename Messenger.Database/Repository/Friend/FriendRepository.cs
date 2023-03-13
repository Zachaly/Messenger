using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public class FriendRepository : KeylessRepositoryBase<Friend, FriendListItem, GetFriendsRequest>, IFriendRepository
    {
        public FriendRepository(ISqlQueryBuilder queryBuilder, IConnectionFactory connectionFactory)
            :base(connectionFactory, queryBuilder)
        {
            Table = "Friend";
            DefaultOrderBy = "[User1Id]";
        }

        public Task DeleteAsync(long user1Id, long user2Id)
        {
            var query = _sqlQueryBuilder.Where(new { User1Id = user1Id, User2Id = user2Id }).BuildDelete("Friend");

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
