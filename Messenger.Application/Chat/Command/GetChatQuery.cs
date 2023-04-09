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
        private IChatRepository _chatRepository;

        public GetChatHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public Task<IEnumerable<ChatModel>> Handle(GetChatQuery request, CancellationToken cancellationToken)
        {
            return _chatRepository.GetAsync(request);
        }
    }
}
