using MediatR;
using Messenger.Application.Command;
using Messenger.Models.Chat;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Hubs
{
    public static class ChatUserConnections
    {
        public static Dictionary<long, List<string>> Users { get; set; } = new Dictionary<long, List<string>>();
    }

    public interface IChatClient
    {
        Task ChatMessageSend(ChatMessageModel message);
        Task ChatMessageRead(long messageId, long userId);
        Task ChatUpdated(ChatModel chat);
        Task ChatUserAdded(ChatUserModel user);
        Task ChatUserRemoved(long id);
        Task ChatUserUpdated(ChatUserModel user);
        Task ChatMessageReactionUpdated(long messageId, long userId, string? reaction);
    }

    [Authorize(AuthenticationSchemes = "Websocket")]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = this.GetUserId();

            var chats = await _mediator.Send(new GetChatQuery { UserId = userId });

            List<string> connectionIds;

            if(!ChatUserConnections.Users.TryGetValue(userId, out connectionIds))
            {
                connectionIds = new List<string>();
                ChatUserConnections.Users.Add(userId, connectionIds);
            }

            connectionIds.Add(Context.ConnectionId);

            foreach(var chat in chats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
            }

            await base.OnConnectedAsync();
        }
    }
}
