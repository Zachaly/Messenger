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
            var friendRequest = await _friendRequestRepository.GetByIdAsync(request.RequestId);
            var receiver = await _userRepository.GetByIdAsync(friendRequest.ReceiverId);

            if (!request.Accepted)
            {
                await _friendRequestRepository.DeleteByIdAsync(request.RequestId);
                return _friendFactory.CreateResponse(false, receiver.Name, friendRequest.ReceiverId);
            }

            await _friendRepository.InsertAsync(_friendFactory.Create(friendRequest.SenderId, friendRequest.ReceiverId));
            await _friendRepository.InsertAsync(_friendFactory.Create(friendRequest.ReceiverId, friendRequest.SenderId));

            await _friendRequestRepository.DeleteByIdAsync(request.RequestId);

            return _friendFactory.CreateResponse(true, receiver.Name, friendRequest.SenderId);
        }
    }
}
