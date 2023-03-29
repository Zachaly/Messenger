using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.Friend.Request;

namespace Messenger.Application.Command
{
    public class GetFriendCountQuery : GetFriendsRequest, IRequest<int>
    {

    }

    public class GetFriendCountHandler : IRequestHandler<GetFriendCountQuery, int>
    {
        private readonly IFriendRepository _friendRepository;

        public GetFriendCountHandler(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }
        public Task<int> Handle(GetFriendCountQuery request, CancellationToken cancellationToken)
        {
            return _friendRepository.GetCount(request);
        }
    }
}
