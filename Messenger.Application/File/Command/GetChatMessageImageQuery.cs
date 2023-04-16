using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;

namespace Messenger.Application.Command
{
    public class GetChatMessageImageQuery : IRequest<FileStream>
    {
        public long ImageId { get; set; }
    }

    public class GetChatMessageImageHandler : IRequestHandler<GetChatMessageImageQuery, FileStream>
    {
        private readonly IFileService _fileService;
        private readonly IChatMessageImageRepository _chatMessageImageRepository;

        public GetChatMessageImageHandler(IFileService fileService, IChatMessageImageRepository chatMessageImageRepository)
        {
            _fileService = fileService;
            _chatMessageImageRepository = chatMessageImageRepository;
        }

        public async Task<FileStream> Handle(GetChatMessageImageQuery request, CancellationToken cancellationToken)
        {
            var image = await _chatMessageImageRepository.GetByIdAsync(request.ImageId);

            return await _fileService.GetChatMessageImage(image.FileName);
        }
    }
}
