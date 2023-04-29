using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;

namespace Messenger.Application.Command
{
    public class GetUserClaimQuery : GetUserClaimRequest, IRequest<IEnumerable<UserClaimModel>>
    {
    }

    public class UserUserClaimHandler : IRequestHandler<GetUserClaimQuery, IEnumerable<UserClaimModel>>
    {
        private readonly IUserClaimRepository _userClaimRepository;

        public UserUserClaimHandler(IUserClaimRepository userClaimRepository)
        {
            _userClaimRepository = userClaimRepository;
        }

        public Task<IEnumerable<UserClaimModel>> Handle(GetUserClaimQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
