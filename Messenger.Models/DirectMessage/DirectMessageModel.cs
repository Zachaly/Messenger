using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.DirectMessage
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [User].[Id]=[DirectMessage].[SenderId]")]
    public class DirectMessageModel
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime Created { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string SenderName { get; set; }
        public long SenderId { get; set; }
        public IEnumerable<long> ImageIds { get; set; }
    }
}
