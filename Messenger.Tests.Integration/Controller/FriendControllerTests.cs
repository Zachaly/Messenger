using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class FriendControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/friend";

        public FriendControllerTests() : base() { }

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authorize();

            var usersToAdd = new List<User>
            {
                new User { Login = "Login", Name = "Test", PasswordHash = "hash" },
                new User { Login = "Login2", Name = "Test2", PasswordHash = "hash" },
                new User { Login = "Login3", Name = "Test3", PasswordHash = "hash" },
                new User { Login = "Login4", Name = "Test4", PasswordHash = "hash" },
                new User { Login = "Login5", Name = "Test5", PasswordHash = "hash" },
            };

            foreach(var user in usersToAdd)
            {
                InsertUser(user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]!=@Id", new { Id = _authorizedUserId });

            const int FriendCount = 3;

            var friends = users.Take(FriendCount);

            var friendsToAdd = friends.Select(x => new Friend { User1Id = _authorizedUserId, User2Id = x.Id });

            foreach(var friend in friendsToAdd)
            {
                ExecuteQuery("INSERT INTO [Friend]([User1Id], [User2Id]) VALUES (@User1Id, @User2Id)", friend);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?UserId={_authorizedUserId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<FriendListItem>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(FriendCount, content.Count());
            Assert.Equivalent(content.Select(x => x.Id), friendsToAdd.Select(x => x.User2Id));
            Assert.Equivalent(content.Select(x => x.Name), friends.Select(x => x.Name));
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await Authorize();

            const long FriendId = 200;

            var friendIds = new long[] { FriendId, 300, 400 };
            var friends = new List<Friend>();

            friends.AddRange(FakeDataFactory.CreateFriends(_authorizedUserId, friendIds));
            friends.AddRange(FakeDataFactory.CreateFriends(friendIds, _authorizedUserId));

            foreach (var friend in friends)
            {
                ExecuteQuery("INSERT INTO [Friend]([User1Id], [User2Id]) VALUES (@User1Id, @User2Id)", friend);
            }

            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{_authorizedUserId}/{FriendId}");

            var currentFriends = GetFromDatabase<Friend>("SELECT * FROM [Friend]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(currentFriends, x => x.User1Id == _authorizedUserId && x.User2Id == FriendId);
            Assert.DoesNotContain(currentFriends, x => x.User1Id == FriendId && x.User2Id == _authorizedUserId);
            Assert.Equal(friends.Count - 2, currentFriends.Count());
        }
    }
}
