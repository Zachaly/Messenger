using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public interface IFriendRequestRepository
    {
        Task<long> InsertFriendRequest(FriendRequest request);
        Task<IEnumerable<FriendRequestModel>> GetFriendRequests(GetFriendsRequestsRequest request);
        Task<FriendRequest> GetRequestById(long id);
        Task<int> GetCount(GetFriendsRequestsRequest request);
        Task DeleteFriendRequestById(long id);
    }
}
