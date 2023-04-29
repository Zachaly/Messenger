using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class ChatMessageControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/chat-message";

        [Fact]
        public async Task GetAsync_Success()
        {
            await Authorize();

            foreach(var user in FakeDataFactory.CreateUsers(15))
            {
                InsertUser(user);
            }

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User]");

            const long ChatId = 2;

            foreach(var message in FakeDataFactory.CreateChatMessages(ChatId, userIds))
            {
                ExecuteQuery("INSERT INTO [ChatMessage]([SenderId], [ChatId], [Created], [Content])" + 
                    " VALUES (@SenderId, @ChatId, @Created, @Content)", message);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?ChatId={ChatId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<ChatMessageModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(10, content.Count());
        }

        [Fact]
        public async Task GetCountAsync_Success()
        {
            await Authorize();

            const int Count = 15;

            foreach (var user in FakeDataFactory.CreateUsers(Count))
            {
                InsertUser(user);
            }

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User] WHERE [Id]!=@Id AND [Login]!=@Login",
                new { Id = _authorizedUserId, Login = _adminLogin });

            const long ChatId = 2;

            foreach (var message in FakeDataFactory.CreateChatMessages(ChatId, userIds))
            {
                ExecuteQuery("INSERT INTO [ChatMessage]([SenderId], [ChatId], [Created], [Content])" +
                    " VALUES (@SenderId, @ChatId, @Created, @Content)", message);
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

            var command = new AddChatMessageCommand { ChatId = 1, SenderId = _authorizedUserId, Content = "message content" };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);

            var messages = GetFromDatabase<ChatMessage>("SELECT * FROM [ChatMessage]");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Single(messages);
            Assert.Contains(messages, x => x.SenderId == command.SenderId && x.ChatId == command.ChatId && x.Content == command.Content);
        }

        
    }
}
