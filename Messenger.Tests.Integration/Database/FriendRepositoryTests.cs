﻿using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Friend.Request;


namespace Messenger.Tests.Integration.Database
{
    public class FriendRepositoryTests : DatabaseTest
    {
        private readonly FriendRepository _repository;

        public FriendRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [Friend]");
            _teardownQueries.Add("TRUNCATE TABLE [User]");

            _repository = new FriendRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetFriendsAsync()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(10));

            var users = await GetAllFromDatabase<User>("User");

            var userId = users.FirstOrDefault().Id;

            var friendIds = users.Where(user => user.Id != userId).Take(5).Select(user => user.Id);

            await InsertFriendsToDatabase(FakeDataFactory.CreateFriends(userId, friendIds));

            var request = new GetFriendsRequest
            {
                UserId = userId
            };

            var res = await _repository.GetAllFriendsAsync(request);

            Assert.Equivalent(res.Select(x => x.Id), friendIds, true);
        }

        [Fact]
        public async Task InsertFriendAsync()
        {
            var friend = new Friend { User1Id = 1, User2Id = 2 };

            await _repository.InsertFriendAsync(friend);

            var friendList = await GetAllFromDatabase<Friend>("Friend");

            Assert.Single(friendList);
            Assert.Contains(friendList, x => x.User1Id == friend.User1Id && x.User2Id == friend.User2Id);
        }
    }
}
