using Messenger.Domain.Entity;

namespace Messenger.Application.Abstraction
{
    public interface IAuthService
    {
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string password, string passwordHash);
        Task<string> GenerateToken(User user);
    }
}
