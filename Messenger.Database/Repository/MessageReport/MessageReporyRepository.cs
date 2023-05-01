using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.MessageReport;
using Messenger.Models.MessageReport.Request;

namespace Messenger.Database.Repository
{
    public class MessageReporyRepository : RepositoryBase<MessageReport, MessageReportModel, GetMessageReportRequest>, IMessageReportRepository
    {
        public MessageReporyRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "MessageReport";
        }

        public Task UpdateAsync(UpdateMessageReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
