using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Friend.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteFriendCommand : DeleteFriendRequest, IRequest<ResponseModel>
    {
    }

    public class DeleteFriendHandler : IRequestHandler<DeleteFriendCommand, ResponseModel>
    {
        private readonly IFriendRepository _friendReposiotory;
        private readonly IResponseFactory _responseFactory;

        public DeleteFriendHandler(IResponseFactory responseFactory, IFriendRepository friendRepository)
        {
            _friendReposiotory = friendRepository;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
