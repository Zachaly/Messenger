using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Database.Repository
{
    public interface IFriendRepository : IKeylessRepository<Friend, FriendListItem, GetFriendsRequest>
    {
        Task DeleteAsync(long user1Id, long user2Id);
    }
}
