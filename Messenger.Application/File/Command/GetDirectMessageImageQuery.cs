using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;

namespace Messenger.Application.Command
{
    public class GetDirectMessageImageQuery : IRequest<FileStream>
    {
        public long ImageId { get; set; }
    }

    public class GetDirectMessageImageHandler : IRequestHandler<GetDirectMessageImageQuery, FileStream>
    {
        private readonly IFileService _fileService;
        private readonly IDirectMessageImageRepository _directMessageImageRepository;

        public GetDirectMessageImageHandler(IFileService fileService, IDirectMessageImageRepository directMessageImageRepository)
        {
            _fileService = fileService;
            _directMessageImageRepository = directMessageImageRepository;
        }

        public Task<FileStream> Handle(GetDirectMessageImageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
