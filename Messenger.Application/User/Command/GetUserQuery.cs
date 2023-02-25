using MediatR;
using Messenger.Models.User;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class GetUsersQuery : GetUserRequest, IRequest<IEnumerable<UserModel>>
    {

    }

    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserModel>>
    {
        public Task<IEnumerable<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
