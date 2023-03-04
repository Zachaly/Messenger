using Messenger.Domain.Entity;
using Messenger.Models.User;

namespace Messenger.Database.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByLogin(string login);  
        Task<IEnumerable<UserModel>> GetUsers(int pageIndex, int pageSize);
        Task<long> InsertUser(User user);
        Task<UserModel> GetUserById(long id);
    }
}
