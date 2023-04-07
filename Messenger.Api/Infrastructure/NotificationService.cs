using Messenger.Api.Hubs;
using Messenger.Application.Abstraction;
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

        public Task DirectMessageReactionChanged(long messageId, string? reaction, long receiverId)
        {
            throw new NotImplementedException();
        }

        public Task ReadDirectMessage(long messageId, long senderId)
        {
            return _directMessageHub.Clients.User(senderId.ToString()).ReadMessage(messageId, true);
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
