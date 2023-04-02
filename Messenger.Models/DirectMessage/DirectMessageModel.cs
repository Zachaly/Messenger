using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.DirectMessage
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [User].[Id]=[DirectMessage].[SenderId]")]
    [Join(Statement = "LEFT OUTER JOIN [DirectMessageImage] ON [DirectMessage].[Id]=[DirectMessageImage].[MessageId]")]
    public class DirectMessageModel
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime Created { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string SenderName { get; set; }
        public long SenderId { get; set; }
        [SqlName(Name = "[DirectMessageImage].*", SkipName = true)]
        public IEnumerable<long> ImageIds { get; set; }
    }
}
