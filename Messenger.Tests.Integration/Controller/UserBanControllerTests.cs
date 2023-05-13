using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.UserBan;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.X86;

namespace Messenger.Tests.Integration.Controller
{
    public class UserBanControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/user-ban";

        [Fact]
        public async Task GetAsync()
        {
            await Authorize();

            var usersToAdd = new List<User>
            {
                new User { Login = "log1", Name = "name2", PasswordHash = "hash" },
                new User { Login = "log1", Name = "name2", PasswordHash = "hash" }
            };

            foreach (var user in usersToAdd)
            {
                InsertUser(user);
            }

            var user1 = GetFromDatabase<User>("SELECT * FROM [User]").First();
            var user2 = GetFromDatabase<User>("SELECT * FROM [User]").Last();

            var bansToAdd = new List<UserBan>
            {
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(1), Start = new DateTime(2000, 1, 1), UserId = user1.Id },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(2), Start = new DateTime(2000, 1, 1), UserId = user2.Id },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(3), Start = new DateTime(2000, 1, 1), UserId = user1.Id },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(4), Start = new DateTime(2000, 1, 1), UserId = user2.Id },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(5), Start = new DateTime(2000, 1, 1), UserId = user1.Id },
            };

            foreach(var ban in bansToAdd)
            {
                ExecuteQuery("INSERT INTO [UserBan]([End], [Start], [UserId]) VALUES(@End, @Start, @UserId)", ban);
            }

            var bans = GetFromDatabase<UserBan>("SELECT * FROM [UserBan]");

            var response = await _httpClient.GetAsync($"{ApiUrl}?UserId={user1.Id}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserBanModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(bans.Where(x => x.UserId == user1.Id).Select(x => x.Id), content.Select(x => x.Id));
            Assert.Equivalent(bans.Where(x => x.UserId == user1.Id).Select(x => x.End), content.Select(x => x.End));
        }

        [Fact]
        public async Task PostAsync()
        {
            await AuthorizeModerator();

            var request = new AddUserBanCommand
            {
                End = DateTime.Now.AddDays(7),
                Start = DateTime.Now,
                UserId = 1
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);

            var bans = GetFromDatabase<UserBan>("SELECT * FROM [UserBan]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(bans);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await AuthorizeModerator();

            var bansToAdd = new List<UserBan>
            {
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(1), Start = new DateTime(2000, 1, 1), UserId = 1 },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(2), Start = new DateTime(2000, 1, 1), UserId = 2 },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(3), Start = new DateTime(2000, 1, 1), UserId = 3 },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(4), Start = new DateTime(2000, 1, 1), UserId = 4 },
                new UserBan { End = new DateTime(2000, 1, 1).AddDays(5), Start = new DateTime(2000, 1, 1), UserId = 5 },
            };

            foreach (var ban in bansToAdd)
            {
                ExecuteQuery("INSERT INTO [UserBan]([End], [Start], [UserId]) VALUES(@End, @Start, @UserId)", ban);
            }

            var banToDelete = GetFromDatabase<UserBan>("SELECT * FROM [UserBan]").Last();

            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{banToDelete.Id}");

            var bans = GetFromDatabase<UserBan>("SELECT * FROM [UserBan]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(bansToAdd.Count() - 1, bans.Count());
            Assert.DoesNotContain(bansToAdd, x => x.Id == banToDelete.Id);
        }
    }
}
