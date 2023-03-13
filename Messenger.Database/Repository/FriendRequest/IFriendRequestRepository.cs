using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public interface IFriendRequestRepository : IRepository<FriendRequest, FriendRequestModel, GetFriendsRequestsRequest>
    {
        Task<FriendRequest> GetByIdAsync(long id);
    }
}
