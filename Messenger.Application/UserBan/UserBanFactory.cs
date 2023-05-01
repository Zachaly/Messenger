using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.UserBan.Request;

namespace Messenger.Application
{
    public class UserBanFactory : IUserBanFactory
    {
        public UserBan Create(AddUserBanRequest request)
            => new UserBan
            {
                End = request.End,
                Start = request.Start,
                UserId = request.UserId,
            };
    }
}
