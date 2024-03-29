﻿using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageRead.Request;

namespace Messenger.Application
{
    public class ChatMessageReadFactory : IChatMessageReadFactory
    {
        public ChatMessageRead Create(AddChatMessageReadRequest request)
            => new ChatMessageRead
            {
                MessageId = request.MessageId,
                UserId = request.UserId,
            };
    }
}
