using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;

namespace Messenger.Tests.Integration.Database
{
    public class ChatMessageReactionRepositoryTests : DatabaseTest
    {
        private readonly ChatMessageReactionRepository _repository;

        public ChatMessageReactionRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [ChatMessageReaction]");
            _repository = new ChatMessageReactionRepository(_connectionFactory, new SqlQueryBuilder());
        }

        [Fact]
        public async Task DeleteAsync()
        {
            const long UserId = 1;
            const long MessageId = 2;

            await InsertChatMessageReactionToDatabase(new ChatMessageReaction { MessageId = MessageId, UserId = UserId, Reaction = "r" });
            await InsertChatMessageReactionToDatabase(new ChatMessageReaction { MessageId = 3, UserId = UserId, Reaction = "r" });

            await _repository.DeleteAsync(UserId, MessageId);

            var reactions = await GetAllFromDatabase<ChatMessageReaction>("ChatMessageReaction");

            Assert.DoesNotContain(reactions, x => x.UserId == UserId && x.MessageId == MessageId);
            Assert.Single(reactions);
        }
    }
}
