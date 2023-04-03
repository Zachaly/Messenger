using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Messenger.Application.Command
{
    public class SaveDirectMessageImagesCommand : IRequest<ResponseModel>
    {
        public long MessageId { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }

    public class SaveDirectMessageImagesHandler : IRequestHandler<SaveDirectMessageImagesCommand, ResponseModel>
    {
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileFactory _fileFactory;
        private readonly IDirectMessageImageRepository _directMessageImageRepository;

        public SaveDirectMessageImagesHandler(IFileService fileService, IResponseFactory responseFactory, IFileFactory fileFactory,
            IDirectMessageImageRepository directMessageImageRepository)
        {
            _fileService = fileService;
            _responseFactory = responseFactory;
            _fileFactory = fileFactory;
            _directMessageImageRepository = directMessageImageRepository;
        }

        public async Task<ResponseModel> Handle(SaveDirectMessageImagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var fileNames = await _fileService.SaveDirectMessageImages(request.Files);

                var images = fileNames.Select(name => _fileFactory.CreateImage(name, request.MessageId));

                foreach(var image in images)
                {
                    await _directMessageImageRepository.InsertAsync(image);
                }

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
