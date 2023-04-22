﻿using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.ChatMessageReaction.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatMessageReactionCommand : AddChatMessageReactionRequest, IRequest<ResponseModel>
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

        public Task<ResponseModel> Handle(AddChatMessageReactionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
