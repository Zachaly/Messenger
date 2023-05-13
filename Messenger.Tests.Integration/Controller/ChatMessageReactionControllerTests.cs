using Messenger.Application.Command;
using Messenger.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Integration.Controller
{
    public class ChatMessageReactionControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/chat-message-reaction";

        [Fact]
        public async Task PostAsync_ReactionDoesNotExists_ReactionAdded_Success()
        {
            await Authorize();

            var command = new AddChatMessageReactionCommand
            {
                ChatId = 3,
                MessageId = 1,
                UserId = 2,
                Reaction = "😊"
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);

            var reactions = GetFromDatabase<ChatMessageReaction>("SELECT * FROM [ChatMessageReaction]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(reactions, x => x.UserId == command.UserId && x.MessageId == command.MessageId && x.Reaction == command.Reaction);
            Assert.Single(reactions);
        }

        [Fact]
        public async Task PostAsync_ReactionExists_ReactionReplaced_Success()
        {
            await Authorize();

            var reaction = new ChatMessageReaction
            {
                MessageId = 1,
                UserId = 2,
                Reaction = "😮"
            };

            ExecuteQuery("INSERT INTO [ChatMessageReaction]([MessageId], [UserId], [Reaction]) VALUES (@MessageId, @UserId, @Reaction)",
                    reaction);

            var command = new AddChatMessageReactionCommand
            {
                ChatId = 3,
                MessageId = 1,
                UserId = 2,
                Reaction = "😊"
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, command);

            var reactions = GetFromDatabase<ChatMessageReaction>("SELECT * FROM [ChatMessageReaction]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(reactions, x => x.UserId == command.UserId && x.MessageId == command.MessageId && x.Reaction == command.Reaction);
            Assert.Single(reactions);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await Authorize();

            var reaction = new ChatMessageReaction
            {
                MessageId = 1,
                UserId = 2,
                Reaction = "😮"
            };

            ExecuteQuery("INSERT INTO [ChatMessageReaction]([MessageId], [UserId], [Reaction]) VALUES (@MessageId, @UserId, @Reaction)",
                    reaction);

            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{reaction.UserId}/{reaction.MessageId}/0");

            var reactions = GetFromDatabase<ChatMessageReaction>("SELECT * FROM [ChatMessageReaction]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Empty(reactions);
        }
    }
}
