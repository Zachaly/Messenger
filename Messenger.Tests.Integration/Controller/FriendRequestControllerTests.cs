using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class FriendRequestControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/friend-request";

        public FriendRequestControllerTests() : base() { }

        private void InsertFriendRequest(FriendRequest friendRequest)
        {
            ExecuteQuery("INSERT INTO [FriendRequest]([SenderId], [ReceiverId], [Created]) VALUES(@SenderId, @ReceiverId, @Created)", friendRequest);
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authorize();

            var request = new AddFriendCommand { SenderId = _authorizedUserId, ReceiverId = 2137 };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);

            var friendRequest = GetFromDatabase<FriendRequest>("SELECT * FROM [FriendRequest]");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Single(friendRequest);
            Assert.Contains(friendRequest, x => x.SenderId == request.SenderId && x.ReceiverId == request.ReceiverId);
        }

        [Fact]
        public async Task PostAsync_RequestExists_Fail()
        {
            await Authorize();

            var request = new FriendRequest { ReceiverId = 2137, SenderId = _authorizedUserId, Created = DateTime.Now };

            InsertFriendRequest(request);

            var command = new AddFriendCommand { SenderId = request.SenderId, ReceiverId = request.ReceiverId };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);

            var friendRequest = GetFromDatabase<FriendRequest>("SELECT * FROM [FriendRequest]");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Single(friendRequest);
            Assert.Contains(friendRequest, x => x.SenderId == command.SenderId && x.ReceiverId == command.ReceiverId);
        }

        [Fact]
        public async Task GetAsync_WithReceiverId_Success()
        {
            await Authorize();

            foreach(var user in FakeDataFactory.CreateUsers(5)) 
            {
                InsertUser(user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]!=@Id", new { Id = _authorizedUserId });

            foreach(var request in FakeDataFactory.CreateFriendRequests(_authorizedUserId, users.Select(x => x.Id)))
            {
                InsertFriendRequest(request);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?ReceiverId={_authorizedUserId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<FriendRequestModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(users.Select(x => x.Id), content.Select(x => x.UserId));
            Assert.Equivalent(users.Select(x => x.Name), content.Select(x => x.Name));
        }

        [Fact]
        public async Task GetAsync_WithSenderId_Success()
        {
            await Authorize();

            foreach (var user in FakeDataFactory.CreateUsers(5))
            {
                InsertUser(user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]!=@Id", new { Id = _authorizedUserId });

            foreach (var request in FakeDataFactory.CreateFriendRequests(users.Select(x => x.Id), _authorizedUserId))
            {
                InsertFriendRequest(request);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?SenderId={_authorizedUserId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<FriendRequestModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(users.Select(x => x.Id), content.Select(x => x.UserId));
            Assert.Equivalent(users.Select(x => x.Name), content.Select(x => x.Name));
        }

        [Fact]
        public async Task GetCountAsync_WithReceiverId_Success()
        {
            await Authorize();

            foreach (var user in FakeDataFactory.CreateUsers(5))
            {
                InsertUser(user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]!=@Id", new { Id = _authorizedUserId });

            foreach (var request in FakeDataFactory.CreateFriendRequests(_authorizedUserId, users.Select(x => x.Id)))
            {
                InsertFriendRequest(request);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}/count?ReceiverId={_authorizedUserId}");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(5, content);
        }

        [Fact]
        public async Task GetCountAsync_WithSenderId_Success()
        {
            await Authorize();

            foreach (var user in FakeDataFactory.CreateUsers(5))
            {
                InsertUser(user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]!=@Id", new { Id = _authorizedUserId });

            foreach (var request in FakeDataFactory.CreateFriendRequests(users.Select(x => x.Id), _authorizedUserId))
            {
                InsertFriendRequest(request);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}/count?SenderId={_authorizedUserId}");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(5, content);
        }

        [Fact]
        public async Task RespondAsync_RequestAccepted()
        {
            await Authorize();

            InsertUser(new User { Name = "test", Login = "login", PasswordHash = "" });

            var user = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]=@Id", new { Id = _authorizedUserId }).First();

            InsertFriendRequest(new FriendRequest { Created = DateTime.Now, ReceiverId = user.Id, SenderId = _authorizedUserId });

            var request = GetFromDatabase<FriendRequest>("SELECT * FROM [FriendRequest]").First();

            var command = new RespondToFriendRequestCommand { Accepted = true, RequestId = request.Id };

            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/respond", command);

            var friends = GetFromDatabase<Friend>("SELECT * FROM [Friend]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(2, friends.Count());
            Assert.Contains(friends, x => x.User1Id == request.SenderId && x.User2Id == request.ReceiverId);
            Assert.Contains(friends, x => x.User1Id == request.ReceiverId && x.User2Id == request.SenderId);
            Assert.Empty(GetFromDatabase<FriendRequest>("SELECT * FROM [FriendRequest]"));
        }

        [Fact]
        public async Task RespondAsync_RequestDenied()
        {
            await Authorize();

            InsertUser(new User { Name = "test", Login = "login", PasswordHash = "" });

            var user = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]=@Id", new { Id = _authorizedUserId }).First();

            InsertFriendRequest(new FriendRequest { Created = DateTime.Now, ReceiverId = user.Id, SenderId = _authorizedUserId });

            var request = GetFromDatabase<FriendRequest>("SELECT * FROM [FriendRequest]").First();

            var command = new RespondToFriendRequestCommand { Accepted = false, RequestId = request.Id };

            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/respond", command);

            var friends = GetFromDatabase<Friend>("SELECT * FROM [Friend]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Empty(friends);
            Assert.Empty(GetFromDatabase<FriendRequest>("SELECT * FROM [FriendRequest]"));
        }
    }
}
