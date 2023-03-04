using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        public Task<IEnumerable<FriendRequestModel>> GetFriendRequests(GetFriendsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<long> InsertFriendRequest(FriendRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<long> UpdateFriendRequest(FriendRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
