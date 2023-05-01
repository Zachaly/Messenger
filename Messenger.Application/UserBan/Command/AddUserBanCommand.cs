using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.UserBan.Request;

namespace Messenger.Application.Command
{
    public class AddUserBanCommand : AddUserBanRequest, IRequest<ResponseModel>
    {
    }

    public class AddUserBanHandler : IRequestHandler<AddUserBanCommand, ResponseModel>
    {
        private readonly IUserBanFactory _userBanFactory;
        private readonly IUserBanRepository _userBanRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public AddUserBanHandler(IUserBanFactory userBanFactory, IUserBanRepository userBanRepository,
            IResponseFactory responseFactory, INotificationService notificationService)
        {
            _userBanFactory = userBanFactory;
            _userBanRepository = userBanRepository;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public Task<ResponseModel> Handle(AddUserBanCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
