using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Tests.Integration.Database
{
    public class ChatMessageRepositoryTests : DatabaseTest
    {
        private readonly ChatMessageRepository _repository;

        public ChatMessageRepositoryTests() : base()
        {
            _repository = new ChatMessageRepository(_connectionFactory, new SqlQueryBuilder());
            _teardownQueries.Add("TRUNCATE TABLE [ChatMessageRead]");
            _teardownQueries.Add("TRUNCATE TABLE [ChatMessage]");
            _teardownQueries.Add("TRUNCATE TABLE [User]");
            _teardownQueries.Add("TRUNCATE TABLE [ChatMessageImage]");
        }

        [Fact]
        public async Task GetAsync_MessageRead_Included()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(10));

            var userIds = (await GetAllFromDatabase<User>("User")).Select(x => x.Id);

            const long ChatId = 1;

            await InsertChatMessagesToDatabase(FakeDataFactory.CreateChatMessages(ChatId, userIds));
            await InsertChatMessagesToDatabase(FakeDataFactory.CreateChatMessages(2, userIds));

            var messageIds = (await GetAllFromDatabase<ChatMessage>("ChatMessage")).Where(x => x.ChatId == ChatId).Select(x => x.Id);

            var readMessageId = messageIds.Last();
            var readerIds = userIds.Take(5);

            var images = FakeDataFactory.CreateChatMessageImages(readMessageId, 10);

            await InsertChatMessageReadsToDatabase(FakeDataFactory.CreateChatMessageReads(readMessageId, readerIds));
            await InsertImagesToDatabase(images);

            var imageIds = (await GetAllFromDatabase<ChatMessageImage>("ChatMessageImage")).Select(x => x.Id);

            var request = new GetChatMessageRequest { ChatId = 1, PageSize = 5 };

            var res = await _repository.GetAsync(request);
            var readMessage = res.First(x => x.Id == readMessageId);

            Assert.Equal(request.PageSize, res.Count());
            Assert.Equivalent(readerIds, readMessage.ReadByIds);
            Assert.Equivalent(imageIds, readMessage.ImageIds);
            Assert.All(res.Select(x => x.Id), id => Assert.Contains(id, messageIds));
        }
    }
}
