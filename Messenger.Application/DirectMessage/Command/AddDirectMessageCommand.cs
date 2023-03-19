using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessage;
using Messenger.Models.DirectMessage.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddDirectMessageCommand : AddDirectMessageRequest, IRequest<ResponseModel> { }

    public class AddDirectMessageHandler : IRequestHandler<AddDirectMessageCommand, ResponseModel>
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IDirectMessageFactory _directMessageFactory;
        private readonly IDirectMessageRepository _directMessageRepository;
        private readonly INotificationService _notificationService;

        public AddDirectMessageHandler(IResponseFactory responseFactory, IDirectMessageFactory directMessageFactory, 
            IDirectMessageRepository directMessageRepository, INotificationService notificationService)
        {
            _responseFactory = responseFactory;
            _directMessageFactory = directMessageFactory;
            _directMessageRepository = directMessageRepository;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(AddDirectMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _directMessageFactory.Create(request);

            var id = await _directMessageRepository.InsertAsync(message);

            var model = await _directMessageRepository.GetByIdAsync(id);

            _notificationService.SendDirectMessage(model, request.SenderId, request.ReceiverId);

            return _responseFactory.CreateSuccess();
        }
    }
}
