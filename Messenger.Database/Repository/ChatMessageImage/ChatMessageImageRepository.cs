﻿using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageImage.Request;

namespace Messenger.Database.Repository
{
    public class ChatMessageImageRepository : RepositoryBase<ChatMessageImage, ChatMessageImage, GetChatMessageImageRequest>,
        IChatMessageImageRepository
    {
        public ChatMessageImageRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "ChatMessageImage";
        }
    }
}
