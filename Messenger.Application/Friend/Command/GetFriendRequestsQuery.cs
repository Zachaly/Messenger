using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;

namespace Messenger.Application.Command
{
    public class GetFriendRequestsQuery : GetFriendsRequestsRequest, IRequest<IEnumerable<FriendRequestModel>>
    {
    }

    public class GetFriendRequestsHandler : IRequestHandler<GetFriendRequestsQuery, IEnumerable<FriendRequestModel>>
    {
        private readonly IFriendRequestRepository _friendRequestRepository;

        public GetFriendRequestsHandler(IFriendRequestRepository friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        public Task<IEnumerable<FriendRequestModel>> Handle(GetFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
