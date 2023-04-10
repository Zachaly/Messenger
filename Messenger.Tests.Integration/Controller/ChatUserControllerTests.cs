using FluentValidation.Results;
using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.ChatUser;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class ChatUserControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/chat-user";

        [Fact]
        public async Task GetAsync_Success()
        {
            await Authorize();

            foreach(var user in FakeDataFactory.CreateUsers(10))
            {
                InsertUser(user);
            }

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User]");
            var chat1Users = userIds.Take(5);
            var chat2Users = userIds.Skip(5).Take(5);

            const long ChatId = 1;

            foreach(var user in FakeDataFactory.CreateChatUsers(ChatId, chat1Users))
            {
                ExecuteQuery("INSERT INTO [ChatUser]([ChatId], [UserId], [IsAdmin]) VALUES (@ChatId, @UserId, @IsAdmin)", user);
            }

            foreach (var user in FakeDataFactory.CreateChatUsers(2, chat2Users))
            {
                ExecuteQuery("INSERT INTO [ChatUser]([ChatId], [UserId], [IsAdmin]) VALUES (@ChatId, @UserId, @IsAdmin)", user);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?ChatId={ChatId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<ChatUserModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(chat1Users, content.Select(x => x.Id));
            Assert.Equal(5, content.Count());
        }

        [Fact]
        public async Task GetCountAsync_Success()
        {
            await Authorize();

            foreach (var user in FakeDataFactory.CreateUsers(10))
            {
                InsertUser(user);
            }

            const int Count = 10;

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User]").Take(Count);

            const long ChatId = 1;

            foreach (var user in FakeDataFactory.CreateChatUsers(ChatId, userIds))
            {
                ExecuteQuery("INSERT INTO [ChatUser]([ChatId], [UserId], [IsAdmin]) VALUES (@ChatId, @UserId, @IsAdmin)", user);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}/count?ChatId={ChatId}");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authorize();

            var command = new AddChatUserCommand { ChatId = 1, UserId = _authorizedUserId };
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);

            var users = GetFromDatabase<ChatUser>("SELECT * FROM [ChatUser]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(users);
            Assert.Contains(users, x => x.UserId == command.UserId && x.ChatId == command.ChatId);
        }

        [Fact]
        public async Task PutAsync_Success()
        {
            await Authorize();

            var chatUser = new ChatUser { ChatId = 1, UserId = _authorizedUserId, IsAdmin = false };

            ExecuteQuery("INSERT INTO [ChatUser]([ChatId], [UserId], [IsAdmin]) VALUES (@ChatId, @UserId, @IsAdmin)", chatUser);

            var command = new UpdateChatUserCommand { ChatId = chatUser.ChatId, UserId = _authorizedUserId, IsAdmin = true };
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, command);

            var users = GetFromDatabase<ChatUser>("SELECT * FROM [ChatUser]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(users);
            Assert.Contains(users, x => x.UserId == command.UserId && x.ChatId == command.ChatId && x.IsAdmin == command.IsAdmin);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await Authorize();

            const long ChatId = 1;
            const long UserId = 2;
            var userIds = new long[] { 1, UserId, 3, 4, 5 };

            foreach (var user in FakeDataFactory.CreateChatUsers(ChatId, userIds))
            {
                ExecuteQuery("INSERT INTO [ChatUser]([ChatId], [UserId], [IsAdmin]) VALUES (@ChatId, @UserId, @IsAdmin)", user);
            }

            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{ChatId}/{UserId}");

            var users = GetFromDatabase<ChatUser>("SELECT * FROM [ChatUser]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(users, x => x.ChatId == ChatId && x.UserId == UserId);
            Assert.Equal(userIds.Length - 1, users.Count());
        }
    }
}
