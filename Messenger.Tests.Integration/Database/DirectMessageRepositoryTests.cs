using Dapper;
using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessage.Request;
using System.Data.SqlClient;

namespace Messenger.Tests.Integration.Database
{
    public class DirectMessageRepositoryTests : DatabaseTest
    {
        private readonly DirectMessageRepository _repository;

        public DirectMessageRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [User]");
            _teardownQueries.Add("TRUNCATE TABLE [DirectMessage]");
            _teardownQueries.Add("TRUNCATE TABLE [DirectMessageImage]");

            _repository = new DirectMessageRepository(_connectionFactory, new SqlQueryBuilder());
        }

        private async Task InsertMessagesAsync(IEnumerable<DirectMessage> messages)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var message in messages)
                {
                    await connection.QueryAsync("INSERT INTO [DirectMessage]([Content], [Created], [SenderId], [ReceiverId], [Read])" +
                        " VALUES(@Content, @Created, @SenderId, @ReceiverId, @Read)", message);
                }
            }
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var message = new DirectMessage { Content = "content", Created = DateTime.Now, Read = false, ReceiverId = 1, SenderId = 2 };

            await InsertMessagesAsync(new List<DirectMessage> { message });

            var id = (await GetAllFromDatabase<DirectMessage>("DirectMessage")).First().Id;

            var request = new UpdateDirectMessageRequest { Id = id, Read = true };

            await _repository.UpdateAsync(request);

            var updatedMessage = (await GetAllFromDatabase<DirectMessage>("DirectMessage")).First();

            Assert.True(updatedMessage.Read);
            Assert.Equal(message.Content, updatedMessage.Content);
            Assert.Equal(message.SenderId, updatedMessage.SenderId);
            Assert.Equal(message.ReceiverId, updatedMessage.ReceiverId);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(2));

            var users = await GetAllFromDatabase<User>("User");

            var sender = users.First();

            await InsertMessagesAsync(FakeDataFactory.CreateMessages(sender.Id, users.ElementAt(1).Id, 5));

            var messageId = (await GetAllFromDatabase<DirectMessage>("DirectMessage")).First().Id;

            var model = await _repository.GetByIdAsync(messageId);

            Assert.Equal(messageId, model.Id);
            Assert.Equal(sender.Name, model.SenderName);
        }

        [Fact]
        public async Task GetAsync()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(2));

            var users = await GetAllFromDatabase<User>("User");

            var sender = users.First();
            var receiver = users.ElementAt(1);

            await InsertMessagesAsync(FakeDataFactory.CreateMessages(sender.Id, receiver.Id, 4));
            await InsertMessagesAsync(FakeDataFactory.CreateMessages(receiver.Id, sender.Id, 4));
            await InsertMessagesAsync(FakeDataFactory.CreateMessages(21, 37, 5));

            var messages = (await GetAllFromDatabase<DirectMessage>("DirectMessage"))
                .Where(x => (x.SenderId == sender.Id || x.SenderId == receiver.Id) && (x.SenderId == receiver.Id || x.ReceiverId == sender.Id));

            var request = new GetDirectMessagesRequest { User1Id = sender.Id, User2Id = 2 };

            var res = await _repository.GetAsync(request);

            Assert.Equal(8, res.Count());
            Assert.Equivalent(messages.Select(x => x.Id), res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetAsync_ImageIdsIncluded()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(2));

            var users = await GetAllFromDatabase<User>("User");

            var sender = users.First();
            var receiver = users.ElementAt(1);

            var message = new DirectMessage { Content = "msg", Created = DateTime.Now, ReceiverId = receiver.Id, SenderId = sender.Id };

            await InsertMessagesAsync(new List<DirectMessage> { message });

            var messageId = (await GetAllFromDatabase<DirectMessage>("DirectMessage")).First().Id;

            await InsertImagesToDatabase(FakeDataFactory.CreateMessageImages(messageId, 5));
            await InsertImagesToDatabase(FakeDataFactory.CreateMessageImages(20, 5));

            var imageIds = (await GetAllFromDatabase<DirectMessageImage>("DirectMessageImage"))
                .Where(x => x.MessageId == messageId)
                .Select(x => x.Id);

            var res = (await _repository.GetAsync(new GetDirectMessagesRequest { Id = messageId })).First();

            Assert.Equivalent(imageIds, res.ImageIds);
        }
    }
}
