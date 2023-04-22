﻿using Messenger.Models.Chat;
using Messenger.Models.ChatMessage;
using Messenger.Models.ChatUser;
using Messenger.Models.DirectMessage;
using Messenger.Models.Friend;

namespace Messenger.Application.Abstraction
{
    public interface INotificationService
    {
        Task SendDirectMessage(DirectMessageModel message, long senderId, long receiverId);
        Task ReadDirectMessage(long messageId, long senderId);
        Task SendFriendRequest(long requestId, long receiverId);
        Task SendFriendRequestResponse(FriendAcceptedResponse response);
        Task DirectMessageReactionChanged(long messageId, string? reaction, long receiverId);

        Task AddedToChat(ChatUserModel user, long chatId);
        Task RemovedFromChat(long userId, long chatId);
        Task ChatUserUpdated(ChatUserModel user, long chatId);
        Task ChatMessageSend(ChatMessageModel message, long chatId);
        Task ChatMessageRead(long chatId, long userId, long messageId);
        Task ChatUpdated(ChatModel chat);
        Task ChatMessageReactionChanged(long chatId, long messageId, string? reaction);
    }
}
