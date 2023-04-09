using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteChatUserCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public long ChatId { get; set; }
    }

    public class DeleteChatUserHandler : IRequestHandler<DeleteChatUserCommand, ResponseModel>
    {
        public DeleteChatUserHandler(IChatUserRepository chatUserRepository, IResponseFactory responseFactory,
            INotificationService notificationService)
        {

        }

        public Task<ResponseModel> Handle(DeleteChatUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
