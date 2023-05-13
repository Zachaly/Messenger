using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatUser.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatUserCommand : AddChatUserRequest, IValidatedRequest
    {
    }

    public class AddChatUserHandler : IRequestHandler<AddChatUserCommand, ResponseModel>
    {
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IChatUserFactory _chatUserFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public AddChatUserHandler(IChatUserRepository chatUserRepository, IChatUserFactory chatUserFactory, IResponseFactory responseFactory,
            INotificationService notificationService)
        {
            _chatUserRepository = chatUserRepository;
            _chatUserFactory = chatUserFactory;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(AddChatUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _chatUserFactory.Create(request);

                await _chatUserRepository.InsertAsync(user);

                var model = (await _chatUserRepository.GetAsync(new GetChatUserRequest { ChatId = request.ChatId, UserId = request.UserId }))
                    .First();

                await _notificationService.AddedToChat(model, request.ChatId);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
