using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatMessageRead.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatMessageReadCommand : AddChatMessageReadRequest, IRequest<ResponseModel>
    {
    }

    public class AddChatMessageReadHandler : IRequestHandler<AddChatMessageReadCommand, ResponseModel>
    {
        public AddChatMessageReadHandler(IChatMessageReadRepository chatMessageReadRepository, IChatMessageReadFactory chatMessageReadFactory,
            IResponseFactory responseFactory, INotificationService notificationService)
        {

        }

        public Task<ResponseModel> Handle(AddChatMessageReadCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
