using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;

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

        public LoginResponse CreateLoginResponse(User user, string token)
            => new LoginResponse
            {
                AuthToken = token,
                UserId = user.Id,
                UserName = user.Name,
            };

        public UserModel CreateModel(User user)
            => new UserModel
            {
                Id = user.Id,
                Name = user.Name,
            };
    }
}
