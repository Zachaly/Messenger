using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Application
{
    public class FriendFactory : IFriendFactory
    {
        public Friend Create(long user1Id, long user2Id)
            => new Friend
            {
                User1Id = user1Id,
                User2Id = user2Id
            };

        public FriendRequest CreateRequest(AddFriendRequest request)
            => new FriendRequest
            {
                Created = DateTime.Now,
                ReceiverId = request.ReceiverId,
                SenderId = request.SenderId,
            };

        public FriendAcceptedResponse CreateResponse(bool accepted, string name, long senderId)
            => new FriendAcceptedResponse
            { 
                Accepted = accepted,
                Name = name,
                SenderId = senderId
            };
    }
}
