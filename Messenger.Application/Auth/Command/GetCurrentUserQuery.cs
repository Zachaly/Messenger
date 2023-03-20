using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.User;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Messenger.Application.Command
{
    public class GetCurrentUserQuery : IRequest<LoginResponse>
    {

    }

    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, LoginResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;

        public GetCurrentUserHandler(IHttpContextAccessor httpContextAccessor, IUserFactory userFactory, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var user = await _userRepository.GetByIdAsync(userId);

            return _userFactory.CreateLoginResponse(user, token);
        }
    }
}
