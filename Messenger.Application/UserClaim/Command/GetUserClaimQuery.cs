using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;

namespace Messenger.Application.Command
{
    public class GetUserClaimQuery : GetUserClaimRequest, IRequest<IEnumerable<UserClaimModel>>
    {
    }

    public class GetUserClaimHandler : IRequestHandler<GetUserClaimQuery, IEnumerable<UserClaimModel>>
    {
        private readonly IUserClaimRepository _userClaimRepository;

        public GetUserClaimHandler(IUserClaimRepository userClaimRepository)
        {
            _userClaimRepository = userClaimRepository;
        }

        public async Task<IEnumerable<UserClaimModel>> Handle(GetUserClaimQuery request, CancellationToken cancellationToken)
        {
            return await _userClaimRepository.GetAsync(request);
        }
    }
}
