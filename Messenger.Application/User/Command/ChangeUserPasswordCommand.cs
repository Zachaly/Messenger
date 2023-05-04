using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class ChangeUserPasswordCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordCommand, ResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IResponseFactory _responseFactory;

        public ChangeUserPasswordHandler(IUserRepository userRepository, IAuthService authService, IResponseFactory responseFactory)
        {
            _userRepository = userRepository;
            _authService = authService;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetEntityByIdAsync(request.UserId);

                if(user is null)
                {
                    return _responseFactory.CreateFailure("No such user");
                }

                if(!await _authService.VerifyPasswordAsync(request.CurrentPassword, user.PasswordHash))
                {
                    return _responseFactory.CreateFailure("Current password does not match");
                }

                var newPassword = await _authService.HashPasswordAsync(request.NewPassword);

                await _userRepository.UpdateAsync(new UpdateUserRequest { Id = request.UserId, PasswordHash = newPassword });

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
