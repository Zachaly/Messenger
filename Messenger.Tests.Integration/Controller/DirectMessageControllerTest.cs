using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage;
using System.Net;
using System.Net.Http.Json;

namespace Messenger.Tests.Integration.Controller
{
    public class DirectMessageControllerTest : ControllerTest
    {
        const string ApiUrl = "/api/direct-message";

        private void InsertMessages(IEnumerable<DirectMessage> messages)
        {
            foreach (var message in messages)
            {
                ExecuteQuery("INSERT INTO [DirectMessage]([Content], [Created], [SenderId], [ReceiverId], [Read])" +
                    " VALUES(@Content, @Created, @SenderId, @ReceiverId, @Read)", message);
            }
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            await Authorize();

            foreach (var user in FakeDataFactory.CreateUsers(2))
            {
                InsertUser(user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]!=@Id", new { Id = _authorizedUserId });

            var sender = users.First();
            var receiver = users.Last();

            InsertMessages(FakeDataFactory.CreateMessages(sender.Id, receiver.Id, 4));
            InsertMessages(FakeDataFactory.CreateMessages(receiver.Id, sender.Id, 4));
            InsertMessages(FakeDataFactory.CreateMessages(21, 37, 5));

            var messages = GetFromDatabase<DirectMessage>("SELECT * FROM [DirectMessage]")
                .Where(x => (x.SenderId == sender.Id && x.ReceiverId == receiver.Id)
                || (x.SenderId == receiver.Id && x.ReceiverId == sender.Id)).ToList();

            var response = await _httpClient.GetAsync($"{ApiUrl}?User1Id={sender.Id}&User2Id={receiver.Id}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<DirectMessageModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(8, content.Count());
            Assert.Equivalent(users.Select(x => x.Name), content.Select(x => x.SenderName));
            Assert.Equivalent(messages.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authorize();

            var request = new AddDirectMessageCommand
            {
                Content = "Content",
                ReceiverId = 1,
                SenderId = _authorizedUserId
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);

            var messages = GetFromDatabase<DirectMessage>("SELECT * FROM [DirectMessage]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(messages, x => x.Content == request.Content && x.ReceiverId == request.ReceiverId && x.SenderId == request.SenderId);
            Assert.Single(messages);
        }

        [Fact]
        public async Task PutAsync_Success()
        {
            await Authorize();

            InsertMessages(FakeDataFactory.CreateMessages(_authorizedUserId, 2, 2));

            var idToUpdate = GetFromDatabase<long>("SELECT [Id] FROM [DirectMessage]").First();

            var request = new UpdateDirectMessageCommand
            {
                Id = idToUpdate,
                Read = true
            };

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, request);

            var messages = GetFromDatabase<DirectMessage>("SELECT * FROM [DirectMessage]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(messages, x => x.Id == idToUpdate && x.Read);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(20, false)]
        public async Task GetCountAsync_Success(int expectedCount, bool read)
        {
            await Authorize();

            const int MessageCount = 20;
            const int SecondUserId = 21;

            InsertMessages(FakeDataFactory.CreateMessages(_authorizedUserId, SecondUserId, MessageCount));
            InsertMessages(FakeDataFactory.CreateMessages(_authorizedUserId, 37, MessageCount));

            var response = await _httpClient.GetAsync($"{ApiUrl}/count?SenderId={_authorizedUserId}&ReceiverId={SecondUserId}&Read={read}");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(expectedCount, content);
        }
    }
}
