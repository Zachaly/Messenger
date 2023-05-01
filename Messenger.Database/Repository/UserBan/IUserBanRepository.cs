using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.UserBan;
using Messenger.Models.UserBan.Request;

namespace Messenger.Database.Repository
{
    public interface IUserBanRepository : IRepository<UserBan, UserBanModel, GetUserBanRequest>
    {

    }
}
