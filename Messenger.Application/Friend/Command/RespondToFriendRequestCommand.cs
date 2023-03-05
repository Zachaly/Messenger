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

        public Task<FriendAcceptedResponse> Handle(RespondToFriendRequestCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
