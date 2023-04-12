using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatMessage.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatMessageCommand : AddChatMessageRequest, IRequest<ResponseModel>
    {

    }

    public class AddChatMessageHandler : IRequestHandler<AddChatMessageCommand, ResponseModel>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatMessageFactory _chatMessageFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public AddChatMessageHandler(IChatMessageRepository chatMessageRepository, IChatMessageFactory chatMessageFactory,
            IResponseFactory responseFactory, INotificationService notificationService)
        {
            _chatMessageRepository = chatMessageRepository;
            _chatMessageFactory = chatMessageFactory;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(AddChatMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var message = _chatMessageFactory.Create(request);

                var messageId = await _chatMessageRepository.InsertAsync(message);

                var model = (await _chatMessageRepository.GetAsync(new GetChatMessageRequest { Id = messageId })).First();

                await _notificationService.ChatMessageSend(model, message.ChatId);

                return _responseFactory.CreateCreatedSuccess(messageId);
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
