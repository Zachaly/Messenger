using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatUser.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Integration.Database
{
    public class ChatUserRepositoryTests : DatabaseTest
    {
        private readonly ChatUserRepository _repository;

        public ChatUserRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [ChatUser]");
            _repository = new ChatUserRepository(_connectionFactory, new SqlQueryBuilder());
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var user = new ChatUser { ChatId = 1, UserId = 2, IsAdmin = false };

            await InsertChatUsersToDatabase(new List<ChatUser> { user });

            var request = new UpdateChatUserRequest { ChatId = 1, UserId = 2, IsAdmin = true };

            await _repository.UpdateAsync(request);

            var users = await GetAllFromDatabase<ChatUser>("ChatUser");

            Assert.Single(users);
            Assert.Contains(users, x => x.ChatId == user.ChatId && x.UserId == user.UserId && x.IsAdmin == request.IsAdmin);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            const long ChatId = 2;
            const long UserId = 3;

            var userIds = new long[] { 1, 2, UserId, 4, 5 };

            await InsertChatUsersToDatabase(FakeDataFactory.CreateChatUsers(ChatId, userIds));

            await _repository.DeleteAsync(UserId, ChatId);

            var users = await GetAllFromDatabase<ChatUser>("ChatUser");

            Assert.DoesNotContain(users, x => x.UserId == UserId && x.ChatId == ChatId);
            Assert.Equal(userIds.Length - 1, users.Count());
        }
    }
}
