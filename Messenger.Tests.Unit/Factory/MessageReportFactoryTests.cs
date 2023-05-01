using Messenger.Application;
using Messenger.Domain.Enum;
using Messenger.Models.MessageReport.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Factory
{
    public class MessageReportFactoryTests
    {
        private readonly MessageReportFactory _factory;

        public MessageReportFactoryTests()
        {
            _factory = new MessageReportFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddMessageReportRequest
            {
                MessageId = 1,
                Reason = "r",
                MessageType = MessageType.Direct,
                ReportedUserId = 2,
                UserId = 3,
            };

            var report = _factory.Create(request);

            Assert.Equal(request.MessageType, report.MessageType);
            Assert.Equal(request.Reason, report.Reason);
            Assert.Equal(request.MessageType, report.MessageType);
            Assert.Equal(request.ReportedUserId, report.ReportedUserId);
            Assert.Equal(request.UserId, report.ReportingUserId);
            Assert.False(report.Resolved);
        }
    }
}
