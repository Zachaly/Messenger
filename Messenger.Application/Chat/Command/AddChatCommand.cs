using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Chat.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatCommand : AddChatRequest, IRequest<ResponseModel>
    {
    }

    public class AddChatHandler : IRequestHandler<AddChatCommand, ResponseModel>
    {
        public AddChatHandler(IChatRepository chatRepository, IChatFactory chatFactory, IResponseFactory responseFactory,
            INotificationService notificationService)
        {

        }

        public Task<ResponseModel> Handle(AddChatCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
