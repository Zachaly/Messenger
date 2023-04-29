using MediatR;
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

        public DeleteUserClamHandler(IUserClaimRepository userClaimRepository)
        {
            _userClaimRepository = userClaimRepository;
        }

        public Task<ResponseModel> Handle(DeleteUserClaimCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
