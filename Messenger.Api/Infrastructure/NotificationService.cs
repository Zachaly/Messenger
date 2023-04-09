using Messenger.Api.Hubs;
using Messenger.Application.Abstraction;
using Messenger.Models.Chat;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatUser;
using Messenger.Models.DirectMessage;
using Messenger.Models.Friend;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Infrastructure
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<DirectMessageHub, IDirectMessageClient> _directMessageHub;
        private readonly IHubContext<FriendHub, IFriendClient> _friendHub;

        public NotificationService(IHubContext<DirectMessageHub, IDirectMessageClient> directMessageHub,
            IHubContext<FriendHub, IFriendClient> friendHub)
        {
            _directMessageHub = directMessageHub;
            _friendHub = friendHub;
        }

        public Task AddedToChat(ChatUserModel user, long chatId)
        {
            throw new NotImplementedException();
        }

        public Task ChatMessageRead(long chatId, long userId, long messageId)
        {
            throw new NotImplementedException();
        }

        public Task ChatMessageSend(ChatMessageModel message, long chatId)
        {
            throw new NotImplementedException();
        }

        public Task ChatUpdated(ChatModel chat)
        {
            throw new NotImplementedException();
        }

        public Task ChatUserUpdated(ChatUserModel user, long chatId)
        {
            throw new NotImplementedException();
        }

        public Task DirectMessageReactionChanged(long messageId, string? reaction, long receiverId)
        {
            return _directMessageHub.Clients.User(receiverId.ToString()).ReactionUpdated(messageId, reaction);
        }

        public Task ReadDirectMessage(long messageId, long senderId)
        {
            return _directMessageHub.Clients.User(senderId.ToString()).ReadMessage(messageId, true);
        }

        public Task RemovedFromChat(long userId, long chatId)
        {
            throw new NotImplementedException();
        }

        public Task SendDirectMessage(DirectMessageModel message, long senderId, long receiverId)
        {
            return _directMessageHub.Clients.Users(new string[] { senderId.ToString(), receiverId.ToString() }).GetMessage(message);
        }

        public Task SendFriendRequest(long requestId, long receiverId)
        {
            return _friendHub.Clients.User(receiverId.ToString()).GetRequest(requestId);
        }

        public Task SendFriendRequestResponse(FriendAcceptedResponse response)
        {
            return _friendHub.Clients.User(response.SenderId.ToString()).GetRequestResponse(response);
        }
    }
}
