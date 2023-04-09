using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.ChatUser;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Application.Command
{
    public class GetChatUserQuery : GetChatUserRequest, IRequest<IEnumerable<ChatUserModel>>
    {

    }

    public class GetChatUserHandler : IRequestHandler<GetChatUserQuery, IEnumerable<ChatUserModel>>
    {
        public GetChatUserHandler(IChatUserRepository chatUserRepository)
        {

        }

        public Task<IEnumerable<ChatUserModel>> Handle(GetChatUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
