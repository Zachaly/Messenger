using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.User.Request;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Command
{
    public class SaveProfileImageCommand : IValidatedRequest
    {
        public long UserId { get; set; }
        public IFormFile? File { get; set; }
    }

    public class SaveProfileImageHandler : IRequestHandler<SaveProfileImageCommand, ResponseModel>
    {
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserRepository _userRepository;

        public SaveProfileImageHandler(IFileService fileService, IResponseFactory responseFactory, IUserRepository userRepository)
        {
            _fileService = fileService;
            _responseFactory = responseFactory;
            _userRepository = userRepository;
        }

        public async Task<ResponseModel> Handle(SaveProfileImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetEntityByIdAsync(request.UserId);

                await _fileService.DeleteProfilePicture(user.ProfileImage);

                var fileName = await _fileService.SaveProfilePicture(request.File);

                await _userRepository.UpdateAsync(new UpdateUserRequest { Id = request.UserId, ProfileImage = fileName });

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
