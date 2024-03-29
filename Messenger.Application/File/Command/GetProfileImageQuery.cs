﻿using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Microsoft.Extensions.Configuration;

namespace Messenger.Application.Command
{
    public class GetProfileImageQuery : IRequest<FileStream>
    {
        public long UserId { get; set; }
    }

    public class GetProfileImageHandler : IRequestHandler<GetProfileImageQuery, FileStream>
    {
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;

        public GetProfileImageHandler(IFileService fileService, IUserRepository userRepository)
        {
            _fileService = fileService;
            _userRepository = userRepository;
        }

        public async Task<FileStream> Handle(GetProfileImageQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetEntityByIdAsync(request.UserId);
            return await _fileService.GetProfilePicture(user.ProfileImage);
        }
    }
}
