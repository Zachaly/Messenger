﻿using Dapper;
using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.Friend.Request;
using System.Data.SqlClient;

namespace Messenger.Tests.Integration.Database
{
    public class FriendRequestRepositoryTests : DatabaseTest
    {
        private readonly FriendRequestRepository _repository;

        public FriendRequestRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [FriendRequest]");
            _teardownQueries.Add("TRUNCATE TABLE [User]");

            _repository = new FriendRequestRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        private async Task InsertFriendRequestsToDatabase(IEnumerable<FriendRequest> friendRequests)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var request in friendRequests)
                {
                    SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
                    await connection.QueryAsync("INSERT INTO [FriendRequest]([ReceiverId], [SenderId], [Created]) VALUES(@ReceiverId, @SenderId, @Created)", request);
                }
            }
        }

        [Fact]
        public async Task GetFriendRequestsAsync_With_ReceiverId()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(10));

            var users = await GetAllFromDatabase<User>("User");

            var user = users.First();

            var friendIds = users.Where(u => u != user).Take(3).Select(x => x.Id);
            var friendNames = users.Where(u => friendIds.Contains(u.Id)).Select(x => x.Name);

            var friendRequests = FakeDataFactory.CreateFriendRequests(user.Id, friendIds).ToList();

            friendRequests.Add(new FriendRequest { SenderId = 420, ReceiverId = 2137, Created = DateTime.Now });

            await InsertFriendRequestsToDatabase(friendRequests);

            var request = new GetFriendsRequestsRequest { ReceiverId = user.Id };

            var res = await _repository.GetFriendRequests(request);

            Assert.Equal(friendIds.Count(), res.Count());
            Assert.Equivalent(friendNames, res.Select(x => x.Name));
        }

        [Fact]
        public async Task GetFriendRequestsAsync_With_SenderId()
        {
            await InsertUsersToDatabase(FakeDataFactory.CreateUsers(10));

            var users = await GetAllFromDatabase<User>("User");

            var user = users.First();

            var friendIds = users.Where(u => u != user).Take(3).Select(x => x.Id);
            var friendNames = users.Where(u => friendIds.Contains(u.Id)).Select(x => x.Name);

            var friendRequests = FakeDataFactory.CreateFriendRequests(friendIds, user.Id).ToList();

            friendRequests.Add(new FriendRequest { SenderId = 2137, ReceiverId = 420, Created = DateTime.Now });

            await InsertFriendRequestsToDatabase(friendRequests);

            var request = new GetFriendsRequestsRequest { SenderId = user.Id };

            var res = await _repository.GetFriendRequests(request);

            Assert.Equal(friendIds.Count(), res.Count());
            Assert.Equivalent(friendNames, res.Select(x => x.Name));
        }

        [Fact]
        public async Task InsertFriendRequestAsync()
        {
            var request = new FriendRequest { Created = DateTime.Now, ReceiverId = 1, SenderId = 2 };

            var res = await _repository.InsertFriendRequest(request);

            var requests = await GetAllFromDatabase<FriendRequest>("FriendRequest");

            Assert.Single(requests);
            Assert.Contains(requests, x => x.Id == res && x.SenderId == request.SenderId && x.ReceiverId == x.ReceiverId);
        }
    }
}
