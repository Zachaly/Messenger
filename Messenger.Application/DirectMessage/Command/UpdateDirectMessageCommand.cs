using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessage.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class UpdateDirectMessageCommand : UpdateDirectMessageRequest, IValidatedRequest { }

    public class UpdateDirectMessageHandler : IRequestHandler<UpdateDirectMessageCommand, ResponseModel>
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IDirectMessageRepository _directMessageRepository;
        private readonly INotificationService _notificationService;

        public UpdateDirectMessageHandler(IResponseFactory responseFactory, IDirectMessageRepository directMessageRepository,
            INotificationService notificationService)
        {
            _responseFactory = responseFactory;
            _directMessageRepository = directMessageRepository;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(UpdateDirectMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _directMessageRepository.UpdateAsync(request);

                var sender = (await _directMessageRepository.GetByIdAsync(request.Id)).SenderId;

                _notificationService.ReadDirectMessage(request.Id, sender);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
