using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.UserClaim;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class UserClaimControllerTests : ControllerTest
    {
        private const string ApiUrl = "/api/user-claim";

        [Fact]
        public async Task GetAsync_Success()
        {
            await AuthorizeAdmin();

            const long UserId = 37;

            var claimsToAdd = new List<UserClaim>
            {
                new UserClaim { UserId = 21, Value = "val" },
                new UserClaim { UserId = 21, Value = "val2" },
                new UserClaim { UserId = 21, Value = "val3" },
                new UserClaim { UserId = UserId, Value = "val4" },
                new UserClaim { UserId = UserId, Value = "val5" },
                new UserClaim { UserId = UserId, Value = "val6" },
            };

            foreach (var claim in claimsToAdd)
            {
                ExecuteQuery("INSERT INTO [UserClaim]([UserId], [Value]) VALUES(@UserId, @Value)", claim);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?UserId={UserId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserClaimModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(claimsToAdd.Where(x => x.UserId == UserId).Select(x => x.Value), content.Select(x => x.Value));
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            await AuthorizeAdmin();

            var request = new AddUserClaimCommand
            {
                UserId = 21,
                Value = "value"
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);

            var claims = GetFromDatabase<UserClaim>("SELECT * FROM [UserClaim]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(claims, x => x.Value == request.Value && x.UserId == request.UserId);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await AuthorizeAdmin();

            var claim = new UserClaim { UserId = 21, Value = "value" };

            var claimsToAdd = new List<UserClaim>
            {
                claim,
                new UserClaim { UserId = 21, Value = "value2" }
            };

            foreach (var c in claimsToAdd)
            {
                ExecuteQuery("INSERT INTO [UserClaim]([UserId], [Value]) VALUES(@UserId, @Value)", c);
            }

            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{claim.UserId}/{claim.Value}");

            var claims = GetFromDatabase<UserClaim>("SELECT * FROM [UserClaim] WHERE [UserId]=@Id", new { Id = claim.UserId });

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(claims, x => x.Value == claim.Value);
            Assert.Single(claims);
        }
    }
}
