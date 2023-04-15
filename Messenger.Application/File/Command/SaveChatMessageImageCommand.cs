using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Application.Command
{
    public class SaveChatMessageImageCommand : IRequest<ResponseModel>
    {
        public long MessageId { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }

    public class SaveChatMessageImageHandler : IRequestHandler<SaveChatMessageImageCommand, ResponseModel>
    {
        private readonly IFileService _fileService;
        private readonly IFileFactory _fileFactory;
        private readonly IChatMessageImageRepository _chatMessageImageRepository;
        private readonly IResponseFactory _responseFactory;

        public SaveChatMessageImageHandler(IFileService fileService, IFileFactory fileFactory,
            IChatMessageImageRepository chatMessageImageRepository, IResponseFactory responseFactory)
        {
            _fileService = fileService;
            _fileFactory = fileFactory;
            _chatMessageImageRepository = chatMessageImageRepository;
            _responseFactory = responseFactory;
        }
        public Task<ResponseModel> Handle(SaveChatMessageImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
