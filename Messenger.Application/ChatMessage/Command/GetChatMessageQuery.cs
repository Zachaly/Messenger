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
        public GetChatMessageHandler(IChatMessageRepository chatMessageRepository)
        {

        }

        public Task<IEnumerable<ChatMessageModel>> Handle(GetChatMessageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
