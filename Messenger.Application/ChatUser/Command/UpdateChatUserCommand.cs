using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatUser.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class UpdateChatUserCommand : UpdateChatUserRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateChatUserHandler : IRequestHandler<UpdateChatUserCommand, ResponseModel>
    {
        public UpdateChatUserHandler(IChatUserRepository chatUserRepository, IResponseFactory responseFactory,
            INotificationService notificationService)
        {

        }

        public Task<ResponseModel> Handle(UpdateChatUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
