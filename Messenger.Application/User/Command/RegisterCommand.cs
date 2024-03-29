﻿using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class RegisterCommand : AddUserRequest, IValidatedRequest { }

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

        public async Task<ResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if(await _userRepository.GetByLoginAsync(request.Login) is not null)
            {
                return _responseFactory.CreateFailure("Login already taken!");
            }

            var passwordHash = await _authService.HashPasswordAsync(request.Password);

            var user = _userFactory.Create(request, passwordHash);

            var id = await _userRepository.InsertAsync(user);

            return _responseFactory.CreateCreatedSuccess(id);
        }
    }
}
