using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Chat.Request;

namespace Messenger.Tests.Integration.Database
{
    public class ChatRepositoryTests : DatabaseTest
    {
        private readonly ChatRepository _repository;

        public ChatRepositoryTests() : base()
        {
            _repository = new ChatRepository(_connectionFactory, new SqlQueryBuilder());
            _teardownQueries.Add("TRUNCATE TABLE [Chat]");
        }

        [Fact]
        public async Task UpdateAsync_Name_Updated()
        {
            var chat = new Chat { CreatorId = 1, Name = "chat" };

            await InsertChatsToDatabase(new List<Chat> { chat });

            var chatId = (await GetAllFromDatabase<Chat>("Chat")).First().Id;

            var request = new UpdateChatRequest { Id = chatId, Name = "new chat" };
            await _repository.UpdateAsync(request);

            var chats = await GetAllFromDatabase<Chat>("Chat");

            Assert.Single(chats);
            Assert.Contains(chats, x => x.Id == chatId && x.Name == request.Name);
        }
    }
}
