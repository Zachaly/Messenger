using Messenger.Domain.Enum;

namespace Messenger.Domain.Entity
{
    public class MessageReport
    {
        public long Id { get; set; }
        public long ReportingUserId { get; set; }
        public long ReportedUserId { get; set; }
        public bool Resolved { get; set; }
        public long AttachedMessageId { get; set; }
        public string Reason { get; set; }
        public DateTime ReportDate { get; set; }
        public MessageType MessageType { get; set; }
    }
}
