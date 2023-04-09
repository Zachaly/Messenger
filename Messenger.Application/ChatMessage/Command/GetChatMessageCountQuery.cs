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
        public GetChatMessageCountHandler(IChatMessageRepository chatMessageRepository)
        {

        }

        public Task<int> Handle(GetChatMessageCountQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
