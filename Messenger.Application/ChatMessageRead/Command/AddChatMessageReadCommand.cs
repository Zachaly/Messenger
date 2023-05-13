using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatMessageRead.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatMessageReadCommand : AddChatMessageReadRequest, IValidatedRequest
    {
        public long ChatId { get; set; }
    }

    public class AddChatMessageReadHandler : IRequestHandler<AddChatMessageReadCommand, ResponseModel>
    {
        private readonly IChatMessageReadRepository _chatMessageReadRepository;
        private readonly IChatMessageReadFactory _chatMessageReadFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;

        public AddChatMessageReadHandler(IChatMessageReadRepository chatMessageReadRepository, IChatMessageReadFactory chatMessageReadFactory,
            IResponseFactory responseFactory, INotificationService notificationService)
        {
            _chatMessageReadRepository = chatMessageReadRepository;
            _chatMessageReadFactory = chatMessageReadFactory;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> Handle(AddChatMessageReadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var read = _chatMessageReadFactory.Create(request);

                await _chatMessageReadRepository.InsertAsync(read);

                await _notificationService.ChatMessageRead(request.ChatId, request.UserId, request.MessageId);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
