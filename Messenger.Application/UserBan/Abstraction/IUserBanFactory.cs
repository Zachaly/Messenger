using Messenger.Domain.Entity;
using Messenger.Models.UserBan.Request;

namespace Messenger.Application.Abstraction
{
    public interface IUserBanFactory
    {
        UserBan Create(AddUserBanRequest request);
    }
}
