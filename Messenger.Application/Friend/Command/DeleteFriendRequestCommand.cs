using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteFriendRequestCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteFriendRequestHandler : IRequestHandler<DeleteFriendRequestCommand, ResponseModel>
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IFriendRequestRepository _friendRequestRepository;

        public DeleteFriendRequestHandler(IResponseFactory responseFactory, IFriendRequestRepository friendRequestRepository)
        {
            _responseFactory = responseFactory;
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task<ResponseModel> Handle(DeleteFriendRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _friendRequestRepository.DeleteFriendRequestById(request.Id);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
