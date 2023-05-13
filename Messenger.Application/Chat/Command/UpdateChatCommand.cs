using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Chat.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class UpdateChatCommand : UpdateChatRequest, IValidatedRequest
    {
    }

    public class UpdateChatHandler : IRequestHandler<UpdateChatCommand, ResponseModel>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public UpdateChatHandler(IChatRepository chatRepository, IResponseFactory responseFactory, INotificationService notificationService)
        {
            _chatRepository = chatRepository;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _chatRepository.UpdateAsync(request);

                var chat = (await _chatRepository.GetAsync(new GetChatRequest { Id = request.Id })).First();

                await _notificationService.ChatUpdated(chat);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
