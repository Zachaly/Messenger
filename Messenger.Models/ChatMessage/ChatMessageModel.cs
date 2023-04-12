using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.ChatMessage
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [User].[Id]=[ChatMessage].[SenderId]")]
    [Join(Statement = "LEFT OUTER JOIN [ChatMessageRead] ON t.[Id]=[ChatMessageRead].[MessageId]", Outside = true)]
    public class ChatMessageModel
    {
        public long Id { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string SenderName { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; }
        [SqlName(Name = "[ChatMessageRead].*", JoinOutside = true)]
        public IEnumerable<long> ReadByIds { get; set; }
    }
}
