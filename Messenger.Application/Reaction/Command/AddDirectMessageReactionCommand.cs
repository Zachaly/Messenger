using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.DirectMessage.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Reaction.Command
{
    public class AddDirectMessageReactionCommand : AddDirectMessageRequest, IRequest<ResponseModel>
    {
        public long ReceiverId { get; set; }
    }

    public class AddDirectMessageReactionHandler : IRequestHandler<AddDirectMessageReactionCommand, ResponseModel>
    {
        private readonly IDirectMessageReactionRepository _reactionRepository;
        private readonly INotificationService _notificationService;
        private readonly IReactionFactory _reactionFactory;

        public AddDirectMessageReactionHandler(IDirectMessageReactionRepository reactionRepository,
            INotificationService notificationService, IReactionFactory reactionFactory)
        {
            _reactionRepository = reactionRepository;
            _notificationService = notificationService;
            _reactionFactory = reactionFactory;
        }

        public Task<ResponseModel> Handle(AddDirectMessageReactionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
