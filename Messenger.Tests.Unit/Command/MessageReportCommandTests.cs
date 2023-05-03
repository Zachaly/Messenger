using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.MessageReport;
using Messenger.Models.MessageReport.Request;
using Messenger.Models.Response;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class MessageReportCommandTests
    {
        [Fact]
        public async Task GetMessageReportQuery_Success()
        {
            var reports = new List<MessageReportModel>
            {
                new MessageReportModel { Id = 1 },
                new MessageReportModel { Id = 2 },
                new MessageReportModel { Id = 3 },
            };
            var repository = new Mock<IMessageReportRepository>();
            repository.Setup(x => x.GetAsync(It.IsAny<GetMessageReportRequest>()))
                .ReturnsAsync(reports);

            var query = new GetMessageReportQuery();
            var res = await new GetMessageReportHandler(repository.Object).Handle(query, default);

            Assert.Equivalent(reports.Select(x => x.Id), res.Select(x => x.Id));
        }

        [Fact]
        public async Task AddMessageReportCommand_Success()
        {
            var reports = new List<MessageReport>();

            var repository = new Mock<IMessageReportRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<MessageReport>()))
                .Callback((MessageReport report) => reports.Add(report));

            var factory = new Mock<IMessageReportFactory>();
            factory.Setup(x => x.Create(It.IsAny<AddMessageReportRequest>()))
                .Returns((AddMessageReportRequest request) => new MessageReport { ReportedUserId = request.UserId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new AddMessageReportCommand { UserId = 1 };
            var res = await new AddMessageReportHandler(repository.Object, factory.Object, responseFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(reports, x => x.ReportedUserId == command.UserId);
        }

        [Fact]
        public async Task AddMessageReportCommand_ExceptionThrown_Fail()
        {
            var reports = new List<MessageReport>();

            const string Error = "err";
            var repository = new Mock<IMessageReportRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<MessageReport>()))
                .Callback((MessageReport report) => throw new Exception(Error));

            var factory = new Mock<IMessageReportFactory>();
            factory.Setup(x => x.Create(It.IsAny<AddMessageReportRequest>()))
                .Returns((AddMessageReportRequest request) => new MessageReport { ReportedUserId = request.UserId });

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var command = new AddMessageReportCommand { UserId = 1 };
            var res = await new AddMessageReportHandler(repository.Object, factory.Object, responseFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.DoesNotContain(reports, x => x.ReportedUserId == command.UserId);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task UpdateMessageReportCommand_Success()
        {
            var report = new MessageReport { Resolved = false };

            var repository = new Mock<IMessageReportRepository>();
            repository.Setup(x => x.UpdateAsync(It.IsAny<UpdateMessageReportRequest>()))
                .Callback((UpdateMessageReportRequest request) => report.Resolved = request.Resolved.GetValueOrDefault());

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new UpdateMessageReportCommand { Resolved = true };
            var res = await new UpdateMessageReportHandler(repository.Object, responseFactory.Object).Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(command.Resolved, report.Resolved);
        }

        [Fact]
        public async Task UpdateMessageReportCommand_ExceptionThrown_Fail()
        {
            var report = new MessageReport { Resolved = false };

            const string Error = "err";

            var repository = new Mock<IMessageReportRepository>();
            repository.Setup(x => x.UpdateAsync(It.IsAny<UpdateMessageReportRequest>()))
                .Callback((UpdateMessageReportRequest request) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var command = new UpdateMessageReportCommand { Resolved = true };
            var res = await new UpdateMessageReportHandler(repository.Object, responseFactory.Object).Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEqual(command.Resolved, report.Resolved);
            Assert.Equal(Error, res.Error);
        }
    }
}
