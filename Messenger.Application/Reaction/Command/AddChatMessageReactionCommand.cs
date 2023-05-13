using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatMessageReaction.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatMessageReactionCommand : AddChatMessageReactionRequest, IValidatedRequest
    {
        public long ChatId { get; set; }
    }

    public class AddChatMessageReactionHandler : IRequestHandler<AddChatMessageReactionCommand, ResponseModel>
    {
        private readonly IChatMessageReactionRepository _chatMessageReactionRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly INotificationService _notificationService;
        private readonly IReactionFactory _reactionFactory;

        public AddChatMessageReactionHandler(IChatMessageReactionRepository chatMessageReactionRepository, IResponseFactory responseFactory,
            INotificationService notificationService, IReactionFactory reactionFactory)
        {
            _chatMessageReactionRepository = chatMessageReactionRepository;
            _responseFactory = responseFactory;
            _notificationService = notificationService;
            _reactionFactory = reactionFactory;
        }

        public async Task<ResponseModel> Handle(AddChatMessageReactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _chatMessageReactionRepository.DeleteAsync(request.UserId, request.MessageId);

                var reaction = _reactionFactory.CreateChatMessageReaction(request);

                await _chatMessageReactionRepository.InsertAsync(reaction);

                await _notificationService.ChatMessageReactionChanged(request.ChatId, reaction.MessageId, reaction.UserId, reaction.Reaction);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
