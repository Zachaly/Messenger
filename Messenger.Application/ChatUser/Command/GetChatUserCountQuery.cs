using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Application.Command
{
    public class GetChatUserCountQuery : GetChatUserRequest, IRequest<int>
    {
    }

    public class GetChatUserCountHandler : IRequestHandler<GetChatUserCountQuery, int>
    {
        private IChatUserRepository _chatUserRepository;

        public GetChatUserCountHandler(IChatUserRepository chatUserRepository)
        {
            _chatUserRepository = chatUserRepository;
        }

        public Task<int> Handle(GetChatUserCountQuery request, CancellationToken cancellationToken)
        {
            return _chatUserRepository.GetCount(request);
        }
    }
}
