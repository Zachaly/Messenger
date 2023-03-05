using Messenger.Application;
using Messenger.Models.Friend.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class FriendFactoryTests
    {
        private readonly FriendFactory _factory;

        public FriendFactoryTests()
        {
            _factory = new FriendFactory();
        }


        [Fact]
        public void Create_Creates_Proper_Entity()
        {
            const int User1Id = 1;
            const int User2Id = 2;

            var friend = _factory.Create(User1Id, User2Id);

            Assert.Equal(User1Id, friend.User1Id);
            Assert.Equal(User2Id, friend.User2Id);
        }

        [Fact]
        public void CreateRequest_Creates_Proper_Entity()
        {
            var addFriendRequest = new AddFriendRequest { SenderId = 1, ReceiverId = 2 };

            var request = _factory.CreateRequest(addFriendRequest);

            Assert.Equal(addFriendRequest.SenderId, request.SenderId);
            Assert.Equal(addFriendRequest.ReceiverId, request.ReceiverId);
        }


        [Fact]
        public void CreateResponse_CreatesProper_Object()
        {
            const bool Accepted = false;
            const string Name = "Name";

            var response = _factory.CreateResponse(Accepted, Name);

            Assert.Equal(Accepted, response.Accepted);
            Assert.Equal(Name, response.Name);
        }
    }
}
