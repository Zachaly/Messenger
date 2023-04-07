using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessageReaction.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Reaction.Command
{
    public class AddDirectMessageReactionCommand : AddDirectMessageReactionRequest, IRequest<ResponseModel>
    {
        public long ReceiverId { get; set; }
    }

    public class AddDirectMessageReactionHandler : IRequestHandler<AddDirectMessageReactionCommand, ResponseModel>
    {
        private readonly IDirectMessageReactionRepository _reactionRepository;
        private readonly INotificationService _notificationService;
        private readonly IReactionFactory _reactionFactory;
        private readonly IResponseFactory _responseFactory;

        public AddDirectMessageReactionHandler(IDirectMessageReactionRepository reactionRepository,
            INotificationService notificationService, IReactionFactory reactionFactory, IResponseFactory responseFactory)
        {
            _reactionRepository = reactionRepository;
            _notificationService = notificationService;
            _reactionFactory = reactionFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(AddDirectMessageReactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var reaction = _reactionFactory.CreateDirectMessageReaction(request);

                await _reactionRepository.DeleteAsync(request.MessageId);

                await _reactionRepository.InsertAsync(reaction);

                await _notificationService.DirectMessageReactionChanged(reaction.MessageId, reaction.Reaction, request.ReceiverId);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
