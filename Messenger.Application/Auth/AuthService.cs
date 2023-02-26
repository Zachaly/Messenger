using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Microsoft.Extensions.Configuration;

namespace Messenger.Application
{
    public class AuthService : IAuthService
    {
        private readonly string _authIssuer;
        private readonly string _authAudience;
        private readonly string _secretKey;
        private readonly string _encryptionKey;

        public AuthService(IConfiguration config)
        {
            _authIssuer = config["Auth:Issuer"];
            _authAudience = config["Auth:Audience"];
            _secretKey = config["Auth:SecretKey"];
            _encryptionKey = config["EncryptionKey"];
        }

        public Task<string> GenerateTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> HashPasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
