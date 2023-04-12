using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var readMessageId = messageIds.First();
            var readerIds = userIds.Take(5);

            await InsertChatMessageReadsToDatabase(FakeDataFactory.CreateChatMessageReads(readMessageId, readerIds));

            var request = new GetChatMessageRequest { ChatId = 1, PageSize = 5 };

            var res = await _repository.GetAsync(request);
            var readMessage = res.First(x => x.Id == readMessageId);

            Assert.Equal(request.PageSize, res.Count());
            Assert.Equivalent(readerIds, readMessage.ReadByIds);
            Assert.All(res.Select(x => x.Id), id => Assert.Contains(id, messageIds));
        }
    }
}
