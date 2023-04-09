using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Chat.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class UpdateChatCommand : UpdateChatRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateChatHandler : IRequestHandler<UpdateChatCommand, ResponseModel>
    {
        public UpdateChatHandler(IChatRepository chatRepository, IResponseFactory responseFactory, INotificationService notificationService)
        {
            
        }

        public Task<ResponseModel> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
