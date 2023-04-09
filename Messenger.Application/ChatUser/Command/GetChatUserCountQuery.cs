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
        public GetChatUserCountHandler(IChatUserRepository chatUserRepository)
        {

        }

        public Task<int> Handle(GetChatUserCountQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
