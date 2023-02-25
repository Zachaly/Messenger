using Messenger.Domain.Entity;
using Messenger.Models.User;

namespace Messenger.Database.Repository
{
    public class UserRepository : IUserRepository
    {
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
