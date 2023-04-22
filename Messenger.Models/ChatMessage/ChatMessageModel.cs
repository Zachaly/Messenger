using Messenger.Domain.SqlAttributes;
using Messenger.Models.ChatMessageReaction;

namespace Messenger.Models.ChatMessage
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [User].[Id]=[ChatMessage].[SenderId]")]
    [Join(Statement = "LEFT OUTER JOIN [ChatMessageRead] ON t.[Id]=[ChatMessageRead].[MessageId]", Outside = true)]
    [Join(Statement = "LEFT OUTER JOIN [ChatMessageImage] ON t.[Id]=[ChatMessageImage].[MessageId]", Outside = true)]
    [Join(Statement = "LEFT OUTER JOIN [ChatMessageReaction] ON t.[Id]=[ChatMessageReaction].[MessageId]", Outside = true)]
    public class ChatMessageModel
    {
        public long Id { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string SenderName { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; }
        [SqlName(Name = "[ChatMessageRead].*", JoinOutside = true)]
        public IEnumerable<long> ReadByIds { get; set; }
        [SqlName(Name = "[ChatMessageImage].*", JoinOutside =true)]
        public IEnumerable<long> ImageIds { get; set; }
        [SqlName(Name = "[ChatMessageReaction].[UserId] as UserId, [ChatMessageReaction].[Reaction] as Reaction", JoinOutside = true)]
        public IEnumerable<ChatMessageReactionModel> Reactions { get; set; }

    }
}
