using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.Chat;
using Messenger.Models.Response;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class ChatControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/chat";

        [Fact]
        public async Task GetAsync_Success()
        {
            await Authorize();

            var chats = FakeDataFactory.CreateChats(_authorizedUserId, 10);

            foreach(var chat in chats)
            {
                ExecuteQuery("INSERT INTO [Chat]([CreatorId], [Name]) VALUES(@CreatorId, @Name)", chat);
            }

            var chatIds = GetFromDatabase<long>("SELECT [Id] FROM [Chat]").Take(5);

            const long UserId = 21;

            var chatUsers = chatIds.Select(x => new ChatUser { ChatId = x, UserId = UserId });

            foreach(var user in chatUsers)
            {
                ExecuteQuery("INSERT INTO [ChatUser]([UserId], [ChatId], [IsAdmin]) VALUES(@UserId, @ChatId, @IsAdmin)", user);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?UserId={UserId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<ChatModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(chatIds, content.Select(x => x.Id));
            Assert.All(content, chat =>
            {
                Assert.Equal(_authUsername, chat.CreatorName);
            });
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authorize();

            var command = new AddChatCommand { Name = "chat", UserId = _authorizedUserId };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);

            var chats = GetFromDatabase<Chat>("SELECT * FROM [Chat]");
            var users = GetFromDatabase<ChatUser>("SELECT * FROM [ChatUser]");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(chats, x => x.CreatorId == command.UserId && x.Name == command.Name);
            Assert.Single(chats);
            Assert.Single(users);
            Assert.Contains(users, x => x.UserId == command.UserId && x.IsAdmin);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_Fail()
        {
            await Authorize();

            var command = new AddChatCommand { Name = new string('a', 51), UserId = _authorizedUserId };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);
            var content = await response.Content.ReadFromJsonAsync<ResponseModel>();

            var chats = GetFromDatabase<Chat>("SELECT * FROM [Chat]");
            var users = GetFromDatabase<ChatUser>("SELECT * FROM [ChatUser]");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.DoesNotContain(chats, x => x.CreatorId == command.UserId && x.Name == command.Name);
            Assert.Empty(chats);
            Assert.Empty(users);
            Assert.DoesNotContain(users, x => x.UserId == command.UserId && x.IsAdmin);
            Assert.Contains(content.Errors.Keys, x => x == "Name");
        }

        [Fact]
        public async Task PutAsync_Success()
        {
            await Authorize();

            var chat = new Chat { CreatorId = _authorizedUserId, Name = "chat" };

            ExecuteQuery("INSERT INTO [Chat]([CreatorId], [Name]) VALUES(@CreatorId, @Name)", chat);

            var chatId = GetFromDatabase<int>("SELECT [Id] FROM [Chat]").First();

            var command = new UpdateChatCommand { Id = chatId, Name = "new chat name" };
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, command);

            var chats = GetFromDatabase<Chat>("SELECT * FROM [Chat]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(chats);
            Assert.Contains(chats, x => x.Id == command.Id && x.Name == command.Name);
        }
    }
}
