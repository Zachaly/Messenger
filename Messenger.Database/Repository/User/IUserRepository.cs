using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;

namespace Messenger.Database.Repository
{
    public interface IUserRepository : IRepository<User, UserModel, GetUserRequest>
    {
        Task<User> GetByLoginAsync(string login);
        Task<UserModel> GetByIdAsync(long id);
        Task UpdateAsync(UpdateUserRequest request);
    }
}
