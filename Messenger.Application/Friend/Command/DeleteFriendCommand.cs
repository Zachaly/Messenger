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

        public async Task<ResponseModel> Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _friendReposiotory.DeleteAsync(request.User1Id, request.User2Id);
                await _friendReposiotory.DeleteAsync(request.User2Id, request.User1Id);
                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
