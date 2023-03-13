using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;
using Messenger.Models.User;

namespace Messenger.Application.Command
{
    public class GetFriendsQuery : GetFriendsRequest, IRequest<IEnumerable<FriendListItem>>
    {
    }

    public class GetFriendsHandler : IRequestHandler<GetFriendsQuery, IEnumerable<FriendListItem>>
    {
        private readonly IFriendRepository _friendRepository;

        public GetFriendsHandler(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public async Task<IEnumerable<FriendListItem>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
        {
            return await _friendRepository.GetAsync(request);
        }
    }
}
