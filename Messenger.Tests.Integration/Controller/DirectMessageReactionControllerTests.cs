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
    public class DirectMessageReactionControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/direct-message-reaction";

        [Fact]
        public async Task PutAsync_ReactionDoesNotExists_ReactionAdded()
        {
            await Authorize();

            var command = new AddDirectMessageReactionCommand
            {
                MessageId = 1,
                Reaction = "😊",
                ReceiverId = 2,
            };

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, command);

            var reactions = GetFromDatabase<DirectMessageReaction>("SELECT * FROM [DirectMessageReaction]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(reactions);
            Assert.Contains(reactions, x => x.Reaction == command.Reaction && x.MessageId == command.MessageId);
        }

        [Fact]
        public async Task PutAsync_ReactionExists_ReactionUpdated()
        {
            await Authorize();

            var originalReaction = new DirectMessageReaction { MessageId = 1, Reaction = "😮" };

            ExecuteQuery("INSERT INTO [DirectMessageReaction]([MessageId], [Reaction]) VALUES(@MessageId, @Reaction)", originalReaction);

            var command = new AddDirectMessageReactionCommand
            {
                MessageId = 1,
                Reaction = "😊",
                ReceiverId = 2,
            };

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, command);

            var reactions = GetFromDatabase<DirectMessageReaction>("SELECT * FROM [DirectMessageReaction]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(reactions);
            Assert.Contains(reactions, x => x.Reaction == command.Reaction && x.MessageId == command.MessageId);
            Assert.DoesNotContain(reactions, x => x.Reaction == originalReaction.Reaction && x.MessageId == originalReaction.MessageId);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await Authorize();

            var reaction = new DirectMessageReaction { MessageId = 1, Reaction = "😮" };

            ExecuteQuery("INSERT INTO [DirectMessageReaction]([MessageId], [Reaction]) VALUES(@MessageId, @Reaction)", reaction);

            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{reaction.MessageId}/2");

            var reactions = GetFromDatabase<DirectMessageReaction>("SELECT * FROM [DirectMessageReaction]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Empty(reactions);
        }
    }
}
