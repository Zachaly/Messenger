using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public interface IFriendRequestRepository
    {
        Task<long> InsertFriendRequest(FriendRequest request);
        Task<long> UpdateFriendRequest(FriendRequest request);
        Task<IEnumerable<FriendRequestModel>> GetFriendRequests(GetFriendsRequest request);
    }
}
