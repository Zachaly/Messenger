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

        public Task<User> GetUserByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserModel>> GetUsers(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<long> InsertUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
