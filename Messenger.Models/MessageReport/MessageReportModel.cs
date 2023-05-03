using Messenger.Domain.Enum;
using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.MessageReport
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [MessageReport].[ReportingUserId]=[User].[Id]")]
    public class MessageReportModel
    {
        public long Id { get; set; }
        public long ReportingUserId { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string ReportingUserName { get; set; }
        public bool Resolved { get; set; }
        public long ReportedUserId { get; set; }
        public long AttachedMessageId { get; set; }
        public string Reason { get; set; }
        public DateTime ReportDate { get; set; }
        public MessageType MessageType { get; set; }
    }
}
