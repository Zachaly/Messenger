using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.MessageReport;
using Messenger.Models.MessageReport.Request;

namespace Messenger.Database.Repository
{
    public interface IMessageReportRepository : IRepository<MessageReport, MessageReportModel, GetMessageReportRequest>
    {
        Task UpdateAsync(UpdateMessageReportRequest request);
    }
}
