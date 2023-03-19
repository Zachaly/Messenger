using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Friend.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class RespondToFriendRequestCommand : RespondToFriendRequest, IRequest<ResponseModel>
    {
    }

    public class RespondToFriendRequestHandler : IRequestHandler<RespondToFriendRequestCommand, ResponseModel>
    {
        private readonly IFriendFactory _friendFactory;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IResponseFactory _responseFactory;

        public RespondToFriendRequestHandler(IFriendFactory friendFactory, IFriendRequestRepository friendRequestRepository,
            IFriendRepository friendRepository, IUserRepository userRepository,
            INotificationService notificationService, IResponseFactory responseFactory)
        {
            _friendFactory = friendFactory;
            _friendRequestRepository = friendRequestRepository;
            _friendRepository = friendRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(RespondToFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var friendRequest = await _friendRequestRepository.GetByIdAsync(request.RequestId);
            var receiver = await _userRepository.GetByIdAsync(friendRequest.ReceiverId);

            if (!request.Accepted)
            {
                await _friendRequestRepository.DeleteByIdAsync(request.RequestId);
                _notificationService.SendFriendRequestResponse(_friendFactory.CreateResponse(false, receiver.Name, friendRequest.SenderId));
                return _responseFactory.CreateSuccess();
            }

            await _friendRepository.InsertAsync(_friendFactory.Create(friendRequest.SenderId, friendRequest.ReceiverId));
            await _friendRepository.InsertAsync(_friendFactory.Create(friendRequest.ReceiverId, friendRequest.SenderId));

            await _friendRequestRepository.DeleteByIdAsync(request.RequestId);

            _notificationService.SendFriendRequestResponse(_friendFactory.CreateResponse(true, receiver.Name, friendRequest.SenderId));

            return _responseFactory.CreateSuccess();
        }
    }
}
