using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.UserClaim.Request;

namespace Messenger.Application.Command
{
    public class AddUserClaimCommand : AddUserClaimRequest, IValidatedRequest
    {
    }

    public class AddUserClaimHandler : IRequestHandler<AddUserClaimCommand, ResponseModel>
    {
        private readonly IUserClaimFactory _userClaimFactory;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public AddUserClaimHandler(IUserClaimFactory userClaimFactory, IUserClaimRepository userClaimRepository,
            IResponseFactory responseFactory, INotificationService notificationService)
        {
            _userClaimFactory = userClaimFactory;
            _userClaimRepository = userClaimRepository;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(AddUserClaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var claim = _userClaimFactory.Create(request);

                await _userClaimRepository.InsertAsync(claim);

                await _notificationService.ClaimAdded(request.UserId, claim.Value);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
