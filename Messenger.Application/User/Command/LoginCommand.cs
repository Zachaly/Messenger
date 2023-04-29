using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.User;
using Messenger.Models.User.Request;
using Messenger.Models.UserClaim.Request;

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
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IUserClaimFactory _userClaimFactory;

        public LoginHandler(IAuthService authService, IUserRepository userRepository,
            IResponseFactory responseFactory, IUserFactory userFactory,
            IUserClaimRepository userClaimRepository, IUserClaimFactory userClaimFactory)
        {
            _authService = authService;
            _userRepository = userRepository;
            _responseFactory = responseFactory;
            _userFactory = userFactory;
            _userClaimRepository = userClaimRepository;
            _userClaimFactory = userClaimFactory;
        }

        public async Task<DataResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByLoginAsync(request.Login);

            if(user == null ||
               !await _authService.VerifyPasswordAsync(request.Password, user.PasswordHash))
            {
                return _responseFactory.CreateFailure<LoginResponse>("Username or password is invalid!");
            }

            var claims = (await _userClaimRepository.GetAsync(new GetUserClaimRequest { UserId = user.Id }))
                .Select(claim => _userClaimFactory.CreateSystemClaimFromModel(claim));

            var response = _userFactory.CreateLoginResponse(user, await _authService.GenerateTokenAsync(user, claims));

            return _responseFactory.CreateSuccess(response);
        }
    }
}
