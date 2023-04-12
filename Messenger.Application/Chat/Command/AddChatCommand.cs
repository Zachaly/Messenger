using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Chat.Request;
using Messenger.Models.ChatUser.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddChatCommand : AddChatRequest, IRequest<ResponseModel>
    {
    }

    public class AddChatHandler : IRequestHandler<AddChatCommand, ResponseModel>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IChatFactory _chatFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IChatUserFactory _chatUserFactory;

        public AddChatHandler(IChatRepository chatRepository, IChatFactory chatFactory, IResponseFactory responseFactory, 
            IChatUserRepository chatUserRepository, IChatUserFactory chatUserFactory)
        {
            _chatRepository = chatRepository;
            _chatFactory = chatFactory;
            _responseFactory = responseFactory;
            _chatUserRepository = chatUserRepository;
            _chatUserFactory = chatUserFactory;
        }

        public async Task<ResponseModel> Handle(AddChatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var chat = _chatFactory.Create(request);

                var chatId = await _chatRepository.InsertAsync(chat);

                var user = _chatUserFactory.Create(chatId, request.UserId);

                await _chatUserRepository.InsertAsync(user);

                return _responseFactory.CreateCreatedSuccess(chatId);
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
