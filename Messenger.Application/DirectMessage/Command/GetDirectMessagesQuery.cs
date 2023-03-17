using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessage;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Application.Command
{
    public class GetDirectMessagesQuery : GetDirectMessagesRequest, IRequest<IEnumerable<DirectMessageModel>> { }

    public class GetDirectMessagesHandler : IRequestHandler<GetDirectMessagesQuery, IEnumerable<DirectMessageModel>>
    {
        private readonly IDirectMessageRepository _directMessageRepository;

        public GetDirectMessagesHandler(IDirectMessageRepository directMessageRepository)
        {
            _directMessageRepository = directMessageRepository;
        }

        public Task<IEnumerable<DirectMessageModel>> Handle(GetDirectMessagesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
