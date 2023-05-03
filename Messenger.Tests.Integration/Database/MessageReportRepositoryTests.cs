using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Domain.Enum;
using Messenger.Models.MessageReport.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Integration.Database
{
    public class MessageReportRepositoryTests : DatabaseTest
    {
        private readonly MessageReportRepository _repository;

        public MessageReportRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [MessageReport]");
            _repository = new MessageReportRepository(_connectionFactory, new SqlQueryBuilder());
        }

        [Fact]
        public async Task UpdateAsync()
        {
            await InsertMessageReportsToDatabase(new List<MessageReport>
            {
                new MessageReport
                {
                    AttachedMessageId = 1,
                    ReportedUserId = 2,
                    ReportingUserId = 3,
                    MessageType = MessageType.Direct,
                    Reason = "res",
                    ReportDate = DateTime.Now,
                    Resolved = false
                }
            });

            var report = (await GetAllFromDatabase<MessageReport>("MessageReport")).First();

            var request = new UpdateMessageReportRequest { Id = report.Id, Resolved = true };

            await _repository.UpdateAsync(request);

            var updatedReport = (await GetAllFromDatabase<MessageReport>("MessageReport")).First();

            Assert.Equal(report.Id, updatedReport.Id);
            Assert.Equal(report.AttachedMessageId, updatedReport.AttachedMessageId);
            Assert.Equal(report.ReportedUserId, updatedReport.ReportedUserId);
            Assert.Equal(report.ReportingUserId, updatedReport.ReportingUserId);
            Assert.Equal(report.MessageType, updatedReport.MessageType);
            Assert.Equal(report.Reason, updatedReport.Reason);
            Assert.Equal(report.ReportDate, updatedReport.ReportDate);
            Assert.Equal(request.Resolved, updatedReport.Resolved);
        }
    }
}
