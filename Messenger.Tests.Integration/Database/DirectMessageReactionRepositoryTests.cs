using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Integration.Database
{
    public class DirectMessageReactionRepositoryTests : DatabaseTest
    {
        private readonly DirectMessageReactionRepository _repository;

        public DirectMessageReactionRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [DirectMessageReaction]");

            _repository = new DirectMessageReactionRepository(_connectionFactory, new SqlQueryBuilder());
        }

        [Fact]
        public async Task DeleteAsync()
        {
            const long MessageId = 2;

            await InsertDirectMessageReactionToDatabase(new DirectMessageReaction { MessageId = 1, Reaction = "r" });
            await InsertDirectMessageReactionToDatabase(new DirectMessageReaction { MessageId = MessageId, Reaction = "r" });

            await _repository.DeleteAsync(MessageId);

            Assert.DoesNotContain(await GetAllFromDatabase<DirectMessageReaction>("DirectMessageReaction"), x => x.MessageId == MessageId);
        }
    }
}
