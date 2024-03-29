﻿using Messenger.Api.Hubs;
using Messenger.Application.Abstraction;
using Messenger.Models.Chat;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatUser;
using Messenger.Models.DirectMessage;
using Messenger.Models.Friend;
using Messenger.Models.UserClaim;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Infrastructure
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<DirectMessageHub, IDirectMessageClient> _directMessageHub;
        private readonly IHubContext<FriendHub, IFriendClient> _friendHub;
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;
        private readonly IHubContext<ClaimHub, IClaimClient> _claimHub;

        public NotificationService(IHubContext<DirectMessageHub, IDirectMessageClient> directMessageHub,
            IHubContext<FriendHub, IFriendClient> friendHub, IHubContext<ChatHub, IChatClient> chatHub,
            IHubContext<ClaimHub, IClaimClient> claimHub)
        {
            _directMessageHub = directMessageHub;
            _friendHub = friendHub;
            _chatHub = chatHub;
            _claimHub = claimHub;
        }

        public async Task AddedToChat(ChatUserModel user, long chatId)
        {
            if(ChatUserConnections.Users.TryGetValue(chatId, out var userConnection))
            {
                foreach(var conn in userConnection)
                {
                    await _chatHub.Groups.AddToGroupAsync(conn, chatId.ToString());
                }
            }
            await _chatHub.Clients.Group(chatId.ToString()).ChatUserAdded(user);
        }

        public Task Banned(long userId, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task ChatMessageReactionChanged(long chatId, long messageId, long userId, string? reaction)
        {
            return _chatHub.Clients.Group(chatId.ToString()).ChatMessageReactionUpdated(messageId, userId, reaction);
        }

        public Task ChatMessageRead(long chatId, long userId, long messageId)
        {
            return _chatHub.Clients.Group(chatId.ToString()).ChatMessageRead(messageId, userId);
        }

        public Task ChatMessageSend(ChatMessageModel message, long chatId)
        {
            return _chatHub.Clients.Group(chatId.ToString()).ChatMessageSend(message);
        }

        public Task ChatUpdated(ChatModel chat)
        {
            return _chatHub.Clients.Group(chat.Id.ToString()).ChatUpdated(chat);
        }

        public Task ChatUserUpdated(ChatUserModel user, long chatId)
        {
            return _chatHub.Clients.Group(chatId.ToString()).ChatUserUpdated(user);
        }

        public Task ClaimAdded(long userId, string claim)
        {
            return _claimHub.Clients.User(userId.ToString()).ClaimAdded(claim);
        }

        public Task DirectMessageReactionChanged(long messageId, string? reaction, long receiverId)
        {
            return _directMessageHub.Clients.User(receiverId.ToString()).ReactionUpdated(messageId, reaction);
        }

        public Task ReadDirectMessage(long messageId, long senderId)
        {
            return _directMessageHub.Clients.User(senderId.ToString()).ReadMessage(messageId, true);
        }

        public async Task RemovedFromChat(long userId, long chatId)
        {   
            await _chatHub.Clients.Group(chatId.ToString()).ChatUserRemoved(userId);
            if (ChatUserConnections.Users.TryGetValue(chatId, out var userConnection))
            {
                foreach (var conn in userConnection)
                {
                    await _chatHub.Groups.RemoveFromGroupAsync(conn, chatId.ToString());
                }
            }
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
