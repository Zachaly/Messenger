using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        public Task<string> GenerateTokenAsync(User user, IEnumerable<Claim> userClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            claims.AddRange(userClaims);

            var token = new JwtSecurityToken(
                _authIssuer,
                _authAudience,
                claims,
                DateTime.Now,
                DateTime.Now.AddDays(365),
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)), SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<string> HashPasswordAsync(string password)
        {
            byte[] bytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.IV = new byte[16];

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(password);
                        }

                        bytes = memoryStream.ToArray();
                    }
                }
            }

            return Task.FromResult(Convert.ToBase64String(bytes));
        }

        public Task<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            string decryptedHash = "";

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.IV = new byte[16];
                var buffer = Convert.FromBase64String(passwordHash);

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            decryptedHash = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return Task.FromResult(password == decryptedHash);
        }
    }
}
