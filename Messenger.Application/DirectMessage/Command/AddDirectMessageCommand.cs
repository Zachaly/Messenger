using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessage;
using Messenger.Models.DirectMessage.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddDirectMessageCommand : AddDirectMessageRequest, IRequest<DataResponseModel<DirectMessageModel>> { }

    public class AddDirectMessageHandler : IRequestHandler<AddDirectMessageCommand, DataResponseModel<DirectMessageModel>>
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IDirectMessageFactory _directMessageFactory;
        private readonly IDirectMessageRepository _directMessageRepository;

        public AddDirectMessageHandler(IResponseFactory responseFactory, IDirectMessageFactory directMessageFactory, 
            IDirectMessageRepository directMessageRepository)
        {
            _responseFactory = responseFactory;
            _directMessageFactory = directMessageFactory;
            _directMessageRepository = directMessageRepository;
        }

        public async Task<DataResponseModel<DirectMessageModel>> Handle(AddDirectMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _directMessageFactory.Create(request);

            var id = await _directMessageRepository.InsertAsync(message);

            var model = await _directMessageRepository.GetByIdAsync(id);

            return _responseFactory.CreateSuccess(model);
        }
    }
}
