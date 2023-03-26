using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Application.Command
{
    public class GetDirectMessageCountQuery : GetDirectMessagesRequest, IRequest<int>
    {
    }

    public class GetDirectMessageCountHandler : IRequestHandler<GetDirectMessageCountQuery, int>
    {
        private readonly IDirectMessageRepository _directMessageRepository;

        public GetDirectMessageCountHandler(IDirectMessageRepository directMessageRepository)
        {
            _directMessageRepository = directMessageRepository;
        }

        public Task<int> Handle(GetDirectMessageCountQuery request, CancellationToken cancellationToken)
        {
            return _directMessageRepository.GetCount(request);
        }
    }
}
