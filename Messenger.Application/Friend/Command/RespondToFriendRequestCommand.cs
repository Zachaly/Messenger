using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Application.Command
{
    public class RespondToFriendRequestCommand : RespondToFriendRequest, IRequest<FriendAcceptedResponse>
    {
    }

    public class RespondToFriendRequestHandler : IRequestHandler<RespondToFriendRequestCommand, FriendAcceptedResponse>
    {
        private readonly IFriendFactory _friendFactory;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IUserRepository _userRepository;

        public RespondToFriendRequestHandler(IFriendFactory friendFactory, IFriendRequestRepository friendRequestRepository,
            IFriendRepository friendRepository, IUserRepository userRepository)
        {
            _friendFactory = friendFactory;
            _friendRequestRepository = friendRequestRepository;
            _friendRepository = friendRepository;
            _userRepository = userRepository;
        }

        public async Task<FriendAcceptedResponse> Handle(RespondToFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var friendRequest = await _friendRequestRepository.GetRequestById(request.RequestId);
            var receiver = await _userRepository.GetUserById(friendRequest.ReceiverId);

            if (!request.Accepted)
            {
                return _friendFactory.CreateResponse(false, receiver.Name);
            }

            await _friendRepository.InsertFriendAsync(_friendFactory.Create(friendRequest.SenderId, friendRequest.ReceiverId));
            await _friendRepository.InsertFriendAsync(_friendFactory.Create(friendRequest.ReceiverId, friendRequest.SenderId));

            return _friendFactory.CreateResponse(true, receiver.Name);
        }
    }
}
