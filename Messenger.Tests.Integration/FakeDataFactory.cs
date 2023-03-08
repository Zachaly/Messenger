using Messenger.Domain.Entity;

namespace Messenger.Tests.Integration
{
    internal static class FakeDataFactory
    {
        public static IEnumerable<User> CreateUsers(int count)
        {
            var users = new List<User>();

            for(int i = 1; i <= count; i++)
            {
                users.Add(new User { Id = i, Login = $"log{i}", Name = $"name{i}", PasswordHash = $"hash{i}" });
            }

            return users;
        }
        
        public static IEnumerable<Friend> CreateFriends(long userId, IEnumerable<long> friendIds)
        {
            return friendIds.Select(id => new Friend { User1Id = userId, User2Id = id });
        }

        public static IEnumerable<FriendRequest> CreateFriendRequests(long receiverId, IEnumerable<long> senderIds)
            => senderIds.Select(id => new FriendRequest { Created = DateTime.Now, SenderId = id, ReceiverId = receiverId });

        public static IEnumerable<FriendRequest> CreateFriendRequests(IEnumerable<long> receiverIds, long senderId)
            => receiverIds.Select(id => new FriendRequest { Created = DateTime.Now, SenderId = senderId, ReceiverId = id });
    }
}
