using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.Friend.Request;

namespace Messenger.Application.Command
{
    public class GetFriendRequestCountQuery : GetFriendsRequestsRequest, IRequest<int>
    {
    }

    public class GetFriendRequestCountHandler : IRequestHandler<GetFriendRequestCountQuery, int>
    {
        private readonly IFriendRequestRepository _friendRequestRepository;

        public GetFriendRequestCountHandler(IFriendRequestRepository friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        public Task<int> Handle(GetFriendRequestCountQuery request, CancellationToken cancellationToken)
        {
            return _friendRequestRepository.GetCount(request);
        }
    }
}
