using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteChatMessageReactionCommand : IRequest<ResponseModel>
    {
        public long ChatId { get; set; }
        public long MessageId { get; set; }
        public long UserId { get; set; }
    }

    public class DeleteChatMessageReactionHandler : IRequestHandler<DeleteChatMessageReactionCommand, ResponseModel>
    {
        private readonly IChatMessageReactionRepository _chatMessageReactionRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public DeleteChatMessageReactionHandler(IChatMessageReactionRepository chatMessageReactionRepository, IResponseFactory responseFactory,
            INotificationService notificationService)
        {
            _chatMessageReactionRepository = chatMessageReactionRepository;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(DeleteChatMessageReactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _chatMessageReactionRepository.DeleteAsync(request.UserId, request.MessageId);

                await _notificationService.ChatMessageReactionChanged(request.ChatId, request.MessageId, null);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
