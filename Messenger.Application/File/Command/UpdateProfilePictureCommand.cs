using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Command
{
    public class UpdateProfilePictureCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public IFormFile File { get; set; }
    }

    public class UpdateProfilePictureHandler : IRequestHandler<UpdateProfilePictureCommand, ResponseModel>
    {
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserRepository _userRepository;

        public UpdateProfilePictureHandler(IFileService fileService, IResponseFactory responseFactory, IUserRepository userRepository)
        {
            _fileService = fileService;
            _responseFactory = responseFactory;
            _userRepository = userRepository;
        }

        public Task<ResponseModel> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
