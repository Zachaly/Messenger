using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;

namespace Messenger.Application
{
    public class AuthService : IAuthService
    {
        public Task<string> GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> HashPassword(string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyPassword(string password, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
