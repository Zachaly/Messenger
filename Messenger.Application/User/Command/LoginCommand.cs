using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.User;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class LoginCommand : LoginRequest, IRequest<DataResponseModel<LoginResponse>>
    {
    }

    public class LoginHandler : IRequestHandler<LoginCommand, DataResponseModel<LoginResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserFactory _userFactory;

        public LoginHandler(IAuthService authService, IUserRepository userRepository,
            IResponseFactory responseFactory, IUserFactory userFactory)
        {
            _authService = authService;
            _userRepository = userRepository;
            _responseFactory = responseFactory;
            _userFactory = userFactory;
        }

        public async Task<DataResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByLogin(request.Login);

            if(user == null ||
               !await _authService.VerifyPasswordAsync(request.Password, user.PasswordHash))
            {
                return _responseFactory.CreateFailure<LoginResponse>("Username or password is invalid!");
            }

            var response = _userFactory.CreateLoginResponse(user, await _authService.GenerateTokenAsync(user));

            return _responseFactory.CreateSuccess(response);
        }
    }
}
