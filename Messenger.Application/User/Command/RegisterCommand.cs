using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class RegisterCommand : AddUserRequest, IRequest<ResponseModel> { }

    public class RegisterHandler : IRequestHandler<RegisterCommand, ResponseModel>
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IAuthService _authService;

        public RegisterHandler(IResponseFactory responseFactory, IUserRepository userRepository,
            IUserFactory userFactory, IAuthService authService)
        {
            _responseFactory = responseFactory;
            _userRepository = userRepository;
            _userFactory = userFactory;
            _authService = authService;
        }

        public Task<ResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
