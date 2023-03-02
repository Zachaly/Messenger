using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.User;

namespace Messenger.Database.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlQueryBuilder _queryBuilder;
        private readonly IConnectionFactory _connectionFactory;

        public UserRepository(ISqlQueryBuilder queryBuilder, IConnectionFactory connectionFactory)
        {
            _queryBuilder = queryBuilder;
            _connectionFactory = connectionFactory;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            var query = _queryBuilder
                    .Select<User>("User")
                    .Where(new { Login = login })
                    .Build();

            using (var connection = _connectionFactory.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(query.Query, query.Params);
            }
        }

        public async Task<IEnumerable<UserModel>> GetUsers(int pageIndex, int pageSize)
        {
            var query = _queryBuilder
                    .Select<UserModel>("User")
                    .AddPagination(pageIndex, pageSize)
                    .Build();

            using(var connection = _connectionFactory.GetConnection())
            {
                return await connection.QueryAsync<UserModel>(query.Query, query.Params);
            }
        }

        public async Task<long> InsertUser(User user)
        {
           var query = _queryBuilder
                    .Insert(user)
                    .Build();

            using (var connection = _connectionFactory.GetConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<long>(query.Query, query.Params);
            }
        }
    }
}
