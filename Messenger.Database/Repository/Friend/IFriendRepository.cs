using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public interface IFriendRepository
    {
        Task InsertFriendAsync(Friend friend);
        Task<IEnumerable<FriendListItem>> GetAllFriendsAsync(GetFriendsRequest request);
        Task DeleteFriendAsync(long user1Id, long user2Id);
    }
}
