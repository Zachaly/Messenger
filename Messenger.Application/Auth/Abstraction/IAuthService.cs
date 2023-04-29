using Messenger.Domain.Entity;
using System.Security.Claims;

namespace Messenger.Application.Abstraction
{
    public interface IAuthService
    {
        Task<string> HashPasswordAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string passwordHash);
        Task<string> GenerateTokenAsync(User user, IEnumerable<Claim> userClaims);
    }
}
