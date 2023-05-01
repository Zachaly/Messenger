using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.MessageReport.Request;

namespace Messenger.Application
{
    public class MessageReportFactory : IMessageReportFactory
    {
        public MessageReport Create(AddMessageReportRequest request)
            => new MessageReport
            {
                AttachedMessageId = request.MessageId,
                MessageType = request.MessageType,
                Reason = request.Reason,
                ReportDate = DateTime.Now,
                ReportedUserId = request.ReportedUserId,
                ReportingUserId = request.UserId,
                Resolved = false
            };
    }
}
