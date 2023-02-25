using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;

namespace Messenger.Application
{
    public class UserFactory : IUserFactory
    {
        public User Create(AddUserRequest request, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public LoginResponse CreateLoginResponse(User user, string token)
        {
            throw new NotImplementedException();
        }

        public UserModel CreateModel(User user)
        {
            throw new NotImplementedException();
        }
    }
}
