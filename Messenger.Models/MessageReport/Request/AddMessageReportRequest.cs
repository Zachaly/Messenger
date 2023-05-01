using Messenger.Domain.Enum;

namespace Messenger.Models.MessageReport.Request
{
    public class AddMessageReportRequest
    {
        public long MessageId { get; set; }
        public string Reason { get; set; }
        public long UserId { get; set; }
        public long ReportedUserId { get; set; }
        public MessageType MessageType { get; set; }
    }
}
