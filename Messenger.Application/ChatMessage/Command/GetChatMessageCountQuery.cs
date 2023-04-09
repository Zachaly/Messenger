using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Application.Command
{
    public class GetChatMessageCountQuery : GetChatMessageRequest, IRequest<int>
    {
    }

    public class GetChatMessageCountHandler : IRequestHandler<GetChatMessageCountQuery, int>
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public GetChatMessageCountHandler(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public Task<int> Handle(GetChatMessageCountQuery request, CancellationToken cancellationToken)
        {
            return _chatMessageRepository.GetCount(request);
        }
    }
}
