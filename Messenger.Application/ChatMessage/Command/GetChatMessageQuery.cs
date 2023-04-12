using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Application.Command
{
    public class GetChatMessageQuery : GetChatMessageRequest, IRequest<IEnumerable<ChatMessageModel>>
    {
    }

    public class GetChatMessageHandler : IRequestHandler<GetChatMessageQuery, IEnumerable<ChatMessageModel>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public GetChatMessageHandler(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public Task<IEnumerable<ChatMessageModel>> Handle(GetChatMessageQuery request, CancellationToken cancellationToken)
        {
            return _chatMessageRepository.GetAsync(request);
        }
    }
}
