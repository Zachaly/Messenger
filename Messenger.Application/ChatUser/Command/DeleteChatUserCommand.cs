using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteChatUserCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public long ChatId { get; set; }
    }

    public class DeleteChatUserHandler : IRequestHandler<DeleteChatUserCommand, ResponseModel>
    {
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public DeleteChatUserHandler(IChatUserRepository chatUserRepository, IResponseFactory responseFactory,
            INotificationService notificationService)
        {
            _chatUserRepository = chatUserRepository;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(DeleteChatUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _chatUserRepository.DeleteAsync(request.UserId, request.ChatId);

                await _notificationService.RemovedFromChat(request.UserId, request.ChatId);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
