using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.Friend.Request;
using Messenger.Models.User;

namespace Messenger.Application.Command
{
    public class GetFriendsQuery : GetFriendsRequest, IRequest<IEnumerable<UserModel>>
    {
    }

    public class GetFriendsHandler : IRequestHandler<GetFriendsQuery, IEnumerable<UserModel>>
    {
        private readonly IFriendRepository _friendRepository;

        public GetFriendsHandler(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public Task<IEnumerable<UserModel>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
