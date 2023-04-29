using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;
using Messenger.Models.UserClaim;

namespace Messenger.Application.Abstraction
{
    public interface IUserFactory
    {
        User Create(AddUserRequest request, string passwordHash);
        UserModel CreateModel(User user);
        LoginResponse CreateLoginResponse(User user, string token, IEnumerable<UserClaimModel> claims);
        LoginResponse CreateLoginResponse(UserModel user, string token, IEnumerable<string> claims);
    }
}
