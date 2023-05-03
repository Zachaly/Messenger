namespace Messenger.Models.MessageReport.Request
{
    public class UpdateMessageReportRequest
    {
        public long Id { get; set; }
        public bool? Resolved { get; set; }
        public long? AttachedMessageId { get; set; }
        public string? Reason { get; set; }
    }
}
