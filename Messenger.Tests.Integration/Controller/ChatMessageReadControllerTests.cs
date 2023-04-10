using Messenger.Application.Command;
using Messenger.Domain.Entity;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class ChatMessageReadControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/chat-message-read";

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authorize();

            var command = new AddChatMessageReadCommand { ChatId = 1, UserId = _authorizedUserId, MessageId = 2 };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);

            var reads = GetFromDatabase<ChatMessageRead>("SELECT * FROM [ChatMessageRead]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(reads, x => x.UserId == command.UserId && x.MessageId == command.MessageId);
            Assert.Single(reads);
        }
    }
}
