using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;

namespace Messenger.Database.Repository
{
    public class UserRepository : RepositoryBase<User, UserModel, GetUserRequest>, IUserRepository
    {
        public UserRepository(ISqlQueryBuilder queryBuilder, IConnectionFactory connectionFactory)
            : base(connectionFactory, queryBuilder)
        {
            Table = "User";
        }

        public Task<UserModel> GetByIdAsync(long id)
        {
            var query = _sqlQueryBuilder.Where(new { Id = id }).BuildSelect<UserModel>(Table);

            return QuerySingleAsync<UserModel>(query.Query, query.Params);
        }

        public Task<User> GetByLoginAsync(string login)
        {
            var query = _sqlQueryBuilder
                    .Where(new { Login = login })
                    .BuildSelect<User>(Table);

            return QuerySingleAsync<User>(query.Query, query.Params);
        }

        public Task<User> GetEntityByIdAsync(long id)
        {
            var query = _sqlQueryBuilder.Where(new { Id = id }).BuildSelect<User>(Table);

            return QuerySingleAsync<User>(query.Query, query.Params);
        }

        public Task UpdateAsync(UpdateUserRequest request)
        {
            var query = _sqlQueryBuilder.Where(new { Id = request.Id }).BuildSet(request, Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
