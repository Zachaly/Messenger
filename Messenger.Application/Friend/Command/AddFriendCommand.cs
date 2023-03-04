using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Friend.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddFriendCommand : AddFriendRequest, IRequest<ResponseModel>
    {
    }

    public class AddFriendHandler : IRequestHandler<AddFriendCommand, ResponseModel>
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendFactory _friendFactory;

        public AddFriendHandler(IFriendRequestRepository friendRequestRepository, IFriendFactory friendFactory, IResponseFactory responseFactory)
        {
            _friendRequestRepository = friendRequestRepository;
            _friendFactory = friendFactory;
        }

        public Task<ResponseModel> Handle(AddFriendCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
