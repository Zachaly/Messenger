using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.Chat;
using Messenger.Models.Chat.Request;

namespace Messenger.Application.Command
{
    public class GetChatQuery : GetChatRequest, IRequest<IEnumerable<ChatModel>>
    {
    }

    public class GetChatHandler : IRequestHandler<GetChatQuery, IEnumerable<ChatModel>>
    {
        public GetChatHandler(IChatRepository chatRepository)
        {

        }

        public Task<IEnumerable<ChatModel>> Handle(GetChatQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
