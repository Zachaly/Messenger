namespace Messenger.Models.MessageReport.Request
{
    public class GetMessageReportRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? ReportingUserId { get; set; }
        public bool? Resolved { get; set; }
        public long? ReportedUserId { get; set; }
        public long? AttachedMessageId { get; set; }
        public string? Reason { get; set; }
        public DateTime? ReportDate { get; set; }
    }
}
