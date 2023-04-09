using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatUser.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatUserCommand : AddChatUserRequest, IRequest<ResponseModel>
    {
    }

    public class AddChatUserHandler : IRequestHandler<AddChatUserCommand, ResponseModel>
    {
        public AddChatUserHandler(IChatUserRepository chatUserRepository, IChatUserFactory chatUserFactory, IResponseFactory responseFactory,
            INotificationService notificationService)
        {

        }

        public Task<ResponseModel> Handle(AddChatUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
