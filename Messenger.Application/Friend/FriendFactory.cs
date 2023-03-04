using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Application
{
    public class FriendFactory : IFriendFactory
    {
        public Friend Create(long user1Id, long user2Id)
        {
            throw new NotImplementedException();
        }

        public FriendRequest CreateRequest(AddFriendRequest request)
        {
            throw new NotImplementedException();
        }

        public FriendAcceptedResponse CreateResponse(bool accepted, string name)
        {
            throw new NotImplementedException();
        }
    }
}
