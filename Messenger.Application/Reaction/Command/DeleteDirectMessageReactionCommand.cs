using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteDirectMessageReactionCommand : IRequest<ResponseModel>
    {
        public long MessageId { get; set; }
        public long ReceiverId { get; set; }
    }

    public class DeleteDirectMessageReactionHandler : IRequestHandler<DeleteDirectMessageReactionCommand, ResponseModel>
    {
        private readonly INotificationService _notificationService;
        private readonly IDirectMessageReactionRepository _reactionRepository;
        private readonly IResponseFactory _responseFactory;

        public DeleteDirectMessageReactionHandler(INotificationService notificationService, IDirectMessageReactionRepository reactionRepository,
            IResponseFactory responseFactory)
        {
            _notificationService = notificationService;
            _reactionRepository = reactionRepository;
            _responseFactory = responseFactory;
        }
        public async Task<ResponseModel> Handle(DeleteDirectMessageReactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _reactionRepository.DeleteAsync(request.MessageId);

                await _notificationService.DirectMessageReactionChanged(request.MessageId, null, request.ReceiverId);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
