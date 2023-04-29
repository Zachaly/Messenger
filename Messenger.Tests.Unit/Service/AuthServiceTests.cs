using Messenger.Application;
using Messenger.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Messenger.Tests.Unit.Service
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _config;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _config = new Mock<IConfiguration>();

            _config.Setup(x => x["Auth:Issuer"]).Returns("localhost");
            _config.Setup(x => x["Auth:Audience"]).Returns("localhost");
            _config.Setup(x => x["Auth:SecretKey"]).Returns("supersecretauthkey");
            _config.Setup(x => x["EncryptionKey"]).Returns(new string('a', 24));

            _service = new AuthService(_config.Object);
        }

        [Fact]
        public async Task HashPasswordAsync_CreatesProperHash()
        {
            const string Password = "zaq1@WSX";

            var hash = await _service.HashPasswordAsync(Password);
            var decryptedHash = "";

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_config.Object["EncryptionKey"]);
                aes.IV = new byte[16];
                var buffer = Convert.FromBase64String(hash);

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

            Assert.NotEqual(Password, hash);
            Assert.Equal(Password, decryptedHash);
        }

        [Fact]
        public async Task ValidatePasswordAsync_Password_Matches_Hash()
        {
            const string Password = "zaq1@WSX";

            byte[] bytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_config.Object["EncryptionKey"]);
                aes.IV = new byte[16];

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(Password);
                        }

                        bytes = memoryStream.ToArray();
                    }
                }
            }
            var hash = Convert.ToBase64String(bytes);

            var res = await _service.VerifyPasswordAsync(Password, hash);

            Assert.True(res);
        }

        [Fact]
        public async Task ValidatePasswordAsync_Password_Does_Not_Match_Hash()
        {
            const string Password = "zaq1@WSX";

            byte[] bytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_config.Object["EncryptionKey"]);
                aes.IV = new byte[16];

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write("XSW@1qaz");
                        }

                        bytes = memoryStream.ToArray();
                    }
                }
            }

            var hash = Convert.ToBase64String(bytes);

            var res = await _service.VerifyPasswordAsync(Password, hash);

            Assert.False(res);
        }

        [Fact]
        public async Task GenerateTokenAsync_Creates_Valid_Token()
        {
            var user = new User { Id = 1, Login = "log", Name = "test" };

            var claim = new Claim("Role", "Admin");

            var token = await _service.GenerateTokenAsync(user, new Claim[] { claim });

            var tokenValidation = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Object["Auth:SecretKey"])),
                ValidIssuer = _config.Object["Auth:Issuer"],
                ValidAudience = _config.Object["Auth:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationResult = tokenHandler.ValidateToken(token, tokenValidation, out SecurityToken validToken);

            Assert.Contains(validationResult.Claims, x => x.Value == user.Id.ToString());
            Assert.Contains(validationResult.Claims, x => x.Value == user.Login);
            Assert.Contains(validationResult.Claims, x => x.Type == claim.Type && x.Value == claim.Value);
        }
    }
}
