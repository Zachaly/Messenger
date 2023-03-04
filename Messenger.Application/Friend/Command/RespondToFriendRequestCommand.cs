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

        public RespondToFriendRequestHandler(IFriendFactory friendFactory, IFriendRequestRepository friendRequestRepository,
            IFriendRepository friendRepository)
        {
            _friendFactory = friendFactory;
            _friendRequestRepository = friendRequestRepository;
            _friendRepository = friendRepository;
        }

        public Task<FriendAcceptedResponse> Handle(RespondToFriendRequestCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
