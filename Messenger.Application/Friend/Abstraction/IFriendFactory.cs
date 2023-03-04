using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Application.Abstraction
{
    public interface IFriendFactory
    {
        FriendRequest CreateRequest(AddFriendRequest request);
        Friend Create(long user1Id, long user2Id);
        FriendAcceptedResponse CreateResponse(bool accepted, string name);
    }
}
