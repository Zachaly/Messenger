using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.UserBan;
using Messenger.Models.UserBan.Request;

namespace Messenger.Application.Command
{
    public class GetUserBanQuery : GetUserBanRequest, IRequest<IEnumerable<UserBanModel>>
    {
    }

    public class GetUserBanHandler : IRequestHandler<GetUserBanQuery, IEnumerable<UserBanModel>>
    {
        private readonly IUserBanRepository _userBanRepository;

        public GetUserBanHandler(IUserBanRepository userBanRepository)
        {
            _userBanRepository = userBanRepository;
        }

        public Task<IEnumerable<UserBanModel>> Handle(GetUserBanQuery request, CancellationToken cancellationToken)
        {
            return _userBanRepository.GetAsync(request);
        }
    }
}
