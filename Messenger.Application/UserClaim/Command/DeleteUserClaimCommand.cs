using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteUserClaimCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public string Claim { get; set; }
    }

    public class DeleteUserClamHandler : IRequestHandler<DeleteUserClaimCommand, ResponseModel>
    {
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IResponseFactory _responseFactory;

        public DeleteUserClamHandler(IUserClaimRepository userClaimRepository, IResponseFactory responseFactory)
        {
            _userClaimRepository = userClaimRepository;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(DeleteUserClaimCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
