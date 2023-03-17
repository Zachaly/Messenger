using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessage.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class UpdateDirectMessageCommand : UpdateDirectMessageRequest, IRequest<ResponseModel> { }

    public class UpdateDirectMessageHandler : IRequestHandler<UpdateDirectMessageCommand, ResponseModel>
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IDirectMessageRepository _directMessageRepository;

        public UpdateDirectMessageHandler(IResponseFactory responseFactory, IDirectMessageRepository directMessageRepository)
        {
            _responseFactory = responseFactory;
            _directMessageRepository = directMessageRepository;
        }

        public Task<ResponseModel> Handle(UpdateDirectMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
