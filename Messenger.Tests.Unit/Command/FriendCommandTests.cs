using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;
using Messenger.Models.Response;
using Messenger.Models.User;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class FriendCommandTests
    {
        [Fact]
        public async Task AddFriendCommand_Success()
        {
            var requests = new List<FriendRequest>();

            var friendFactory = new Mock<IFriendFactory>();
            friendFactory.Setup(x => x.CreateRequest(It.IsAny<AddFriendRequest>()))
                .Returns((AddFriendRequest request) => new FriendRequest { ReceiverId = request.ReceiverId, SenderId = request.SenderId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateCreatedSuccess(It.IsAny<long>()))
                .Returns(new ResponseModel { Success = true, NewEntityId = 1 });

            var friendRepository = new Mock<IFriendRequestRepository>();
            friendRepository.Setup(x => x.InsertFriendRequest(It.IsAny<FriendRequest>()))
                .Callback((FriendRequest request) =>
                {
                    requests.Add(request);
                });

            friendRepository.Setup(x => x.GetCount(It.IsAny<GetFriendsRequestsRequest>()))
                .ReturnsAsync(0);

            var request = new AddFriendCommand { SenderId = 1, ReceiverId = 2 };

            var res = await new AddFriendHandler(friendRepository.Object, friendFactory.Object, responseFactory.Object)
                    .Handle(request, default);

            Assert.True(res.Success);
            Assert.Single(requests);
        }

        [Fact]
        public async Task AddFriendCommand_RequestExists_Fail()
        {
            var requests = new List<FriendRequest>();

            var friendFactory = new Mock<IFriendFactory>();
            friendFactory.Setup(x => x.CreateRequest(It.IsAny<AddFriendRequest>()))
                .Returns((AddFriendRequest request) => new FriendRequest { ReceiverId = request.ReceiverId, SenderId = request.SenderId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns(new ResponseModel { Success = false });

            var friendRepository = new Mock<IFriendRequestRepository>();
            friendRepository.Setup(x => x.InsertFriendRequest(It.IsAny<FriendRequest>()))
                .Callback((FriendRequest request) =>
                {
                    requests.Add(request);
                });

            friendRepository.Setup(x => x.GetCount(It.IsAny<GetFriendsRequestsRequest>()))
                .ReturnsAsync(1);

            var request = new AddFriendCommand { SenderId = 1, ReceiverId = 2 };

            var res = await new AddFriendHandler(friendRepository.Object, friendFactory.Object, responseFactory.Object)
                    .Handle(request, default);

            Assert.False(res.Success);
            Assert.Empty(requests);
        }

        [Fact]
        public async Task GetFriendRequestsQuery_ReturnsRequests()
        {
            var requests = new List<FriendRequestModel>
            {
                new FriendRequestModel { Id = 1, },
                new FriendRequestModel { Id = 2, },
                new FriendRequestModel { Id = 3, },
                new FriendRequestModel { Id = 4, },
            };

            var friendRepository = new Mock<IFriendRequestRepository>();

            friendRepository.Setup(x => x.GetFriendRequests(It.IsAny<GetFriendsRequestsRequest>()))
                .ReturnsAsync(requests);

            var query = new GetFriendRequestsQuery { SenderId = 1, ReceiverId = 2 };
            var res = await new GetFriendRequestsHandler(friendRepository.Object).Handle(query, default);

            Assert.Equivalent(requests, res);
        }

        [Fact]
        public async Task GetFriendsQuery_ReturnsFriends()
        {
            var friends = new List<FriendListItem>
            {
                new FriendListItem { Id = 1, },
                new FriendListItem { Id = 2, },
                new FriendListItem { Id = 3, },
                new FriendListItem { Id = 4, }
            };

            var friendRepository = new Mock<IFriendRepository>();
            friendRepository.Setup(x => x.GetAllFriendsAsync(It.IsAny<GetFriendsRequest>()))
                .ReturnsAsync(friends);

            var query = new GetFriendsQuery { UserId = 1 };

            var res = await new GetFriendsHandler(friendRepository.Object).Handle(query, default);

            Assert.Equivalent(friends, res);
        }

        [Fact]
        public async Task RespondToFriendRequestCommand_FriendAccepted_Success()
        {
            var friends = new List<Friend>();

            var request = new FriendRequest { Id = 1, SenderId = 2, ReceiverId = 3 };

            var receiver = new UserModel { Name = "name" }; 

            var friendRepository = new Mock<IFriendRepository>();

            friendRepository.Setup(x => x.InsertFriendAsync(It.IsAny<Friend>()))
                .Callback((Friend friend) => friends.Add(friend));

            var friendRequestRepository = new Mock<IFriendRequestRepository>();

            friendRequestRepository.Setup(x => x.GetRequestById(It.IsAny<long>()))
                .ReturnsAsync(request);

            friendRequestRepository.Setup(x => x.DeleteFriendRequestById(It.IsAny<long>()));
                
            var friendFactory = new Mock<IFriendFactory>();

            friendFactory.Setup(x => x.CreateResponse(It.IsAny<bool>(), It.IsAny<string>()))
                .Returns((bool accepted, string name) => new FriendAcceptedResponse { Accepted = accepted, Name = name });

            friendFactory.Setup(x => x.Create(It.IsAny<long>(), It.IsAny<long>()))
                .Returns((long id1, long id2) => new Friend { User1Id = id1, User2Id = id2 });

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUserById(It.IsAny<long>()))
                .ReturnsAsync(receiver);

            var command = new RespondToFriendRequestCommand
            {
                RequestId = 1,
                Accepted = true
            };

            var res = await new RespondToFriendRequestHandler(friendFactory.Object, friendRequestRepository.Object,
                friendRepository.Object, userRepository.Object)
                .Handle(command, default);

            Assert.True(res.Accepted);
            Assert.Equal(receiver.Name, res.Name);
            Assert.Equal(2, friends.Count);
        }

        [Fact]
        public async Task RespondToFriendRequestCommand_FriendRejected_Success()
        {
            var receiver = new UserModel { Name = "name" };
            var request = new FriendRequest { Id = 1, SenderId = 2, ReceiverId = 3 };

            var friendRepository = new Mock<IFriendRepository>();

            var friendRequestRepository = new Mock<IFriendRequestRepository>();

            friendRequestRepository.Setup(x => x.GetRequestById(It.IsAny<long>()))
                .ReturnsAsync(request);
            friendRequestRepository.Setup(x => x.DeleteFriendRequestById(It.IsAny<long>()));

            var friendFactory = new Mock<IFriendFactory>();

            friendFactory.Setup(x => x.CreateResponse(It.IsAny<bool>(), It.IsAny<string>()))
                .Returns((bool accepted, string name) => new FriendAcceptedResponse { Accepted = accepted, Name = name });

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUserById(It.IsAny<long>()))
                .ReturnsAsync(receiver);

            var command = new RespondToFriendRequestCommand
            {
                RequestId = 1,
                Accepted = false
            };

            var res = await new RespondToFriendRequestHandler(friendFactory.Object, friendRequestRepository.Object,
                friendRepository.Object, userRepository.Object)
                .Handle(command, default);

            Assert.False(res.Accepted);
            Assert.Equal(receiver.Name, res.Name);
        }
    }
}
