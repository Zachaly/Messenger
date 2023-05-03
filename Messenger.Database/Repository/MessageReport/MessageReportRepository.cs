using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.MessageReport;
using Messenger.Models.MessageReport.Request;

namespace Messenger.Database.Repository
{
    public class MessageReportRepository : RepositoryBase<MessageReport, MessageReportModel, GetMessageReportRequest>, IMessageReportRepository
    {
        public MessageReportRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "MessageReport";
        }

        public Task UpdateAsync(UpdateMessageReportRequest request)
        {
            var query = _sqlQueryBuilder.Where(new { Id = request.Id }).BuildSet(request, Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }
    }
}
