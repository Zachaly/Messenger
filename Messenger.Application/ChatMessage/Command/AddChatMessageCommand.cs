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
        public AddChatMessageHandler(IChatMessageRepository chatMessageRepository, IChatMessageFactory chatMessageFactory,
            IResponseFactory responseFactory, INotificationService notificationService)
        {

        }

        public Task<ResponseModel> Handle(AddChatMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
