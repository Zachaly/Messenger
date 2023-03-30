using MediatR;
using Messenger.Application.Abstraction;
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

        public SaveDirectMessageImagesHandler(IFileService fileService, IResponseFactory responseFactory)
        {
            _fileService = fileService;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(SaveDirectMessageImagesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
