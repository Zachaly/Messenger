using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatUser.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class UpdateChatUserCommand : UpdateChatUserRequest, IValidatedRequest
    {
    }

    public class UpdateChatUserHandler : IRequestHandler<UpdateChatUserCommand, ResponseModel>
    {
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public UpdateChatUserHandler(IChatUserRepository chatUserRepository, IResponseFactory responseFactory,
            INotificationService notificationService)
        {
            _chatUserRepository = chatUserRepository;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(UpdateChatUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _chatUserRepository.UpdateAsync(request);

                var model = (await _chatUserRepository.GetAsync(new GetChatUserRequest { ChatId = request.ChatId, UserId = request.UserId }))
                    .First();

                await _notificationService.ChatUserUpdated(model, request.ChatId);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
