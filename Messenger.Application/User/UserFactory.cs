using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;
using Messenger.Models.UserClaim;

namespace Messenger.Application
{
    public class UserFactory : IUserFactory
    {
        public User Create(AddUserRequest request, string passwordHash)
            => new User
            {
                Login = request.Login,
                Name = request.Name,
                PasswordHash = passwordHash
            };

        public LoginResponse CreateLoginResponse(User user, string token, IEnumerable<UserClaimModel> claims)
            => new LoginResponse
            {
                AuthToken = token,
                UserId = user.Id,
                UserName = user.Name,
                Claims = claims.Select(claim => claim.Value)
            };

        public LoginResponse CreateLoginResponse(UserModel user, string token, IEnumerable<string> claims)
           => new LoginResponse
           {
               AuthToken = token,
               UserId = user.Id,
               UserName = user.Name,
               Claims = claims
           };

        public UserModel CreateModel(User user)
            => new UserModel
            {
                Id = user.Id,
                Name = user.Name,
            };
    }
}
