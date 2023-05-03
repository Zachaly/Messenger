using Messenger.Domain.Entity;
using Messenger.Models.MessageReport.Request;

namespace Messenger.Application.Abstraction
{
    public interface IMessageReportFactory
    {
        public MessageReport Create(AddMessageReportRequest request);
    }
}
