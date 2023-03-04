using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.User;

namespace Messenger.Application.User.Command
{
    public class GetUserByIdQuery : IRequest<UserModel>
    {
        public long UserId { get; set; }
    }

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserModel>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
