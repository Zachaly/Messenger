using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.User;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class GetUsersQuery : GetUserRequest, IRequest<IEnumerable<UserModel>>
    {

    }

    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserModel>>
    {
        private readonly IUserRepository _repository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public Task<IEnumerable<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
