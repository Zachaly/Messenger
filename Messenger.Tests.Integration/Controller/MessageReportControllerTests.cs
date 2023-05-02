using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Domain.Enum;
using Messenger.Models.MessageReport;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Messenger.Tests.Integration.Controller
{
    public class MessageReportControllerTests : ControllerTest
    {
        const string ApiUrl = "/api/message-report";

        [Fact]
        public async Task GetAsync_Success()
        {
            await AuthorizeModerator();

            InsertUser(new User { Login = "log", Name = "name", PasswordHash = "hash" });

            var user = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Name]!=@Name", new { Name = _authUsername }).First();

            var reportsToAdd = new List<MessageReport>
            {
                new MessageReport
                {
                    AttachedMessageId = 1,
                    MessageType = MessageType.Direct,
                    Reason = "res1",
                    ReportDate = DateTime.Now,
                    ReportedUserId = user.Id,
                    ReportingUserId = _authorizedUserId,
                    Resolved = false
                },
                new MessageReport
                {
                    AttachedMessageId = 2,
                    MessageType = MessageType.Direct,
                    Reason = "res2",
                    ReportDate = DateTime.Now,
                    ReportedUserId = _authorizedUserId,
                    ReportingUserId = user.Id,
                    Resolved = false
                },
                new MessageReport
                {
                    AttachedMessageId = 3,
                    MessageType = MessageType.Direct,
                    Reason = "res3",
                    ReportDate = DateTime.Now,
                    ReportedUserId = user.Id,
                    ReportingUserId = _authorizedUserId,
                    Resolved = true
                },
                new MessageReport
                {
                    AttachedMessageId = 4,
                    MessageType = MessageType.Direct,
                    Reason = "res4",
                    ReportDate = DateTime.Now,
                    ReportedUserId = user.Id,
                    ReportingUserId = _authorizedUserId,
                    Resolved = false
                },
            };

            foreach(var report in reportsToAdd)
            {
                ExecuteQuery(@"INSERT INTO [MessageReport]([ReportingUserId], [ReportedUserId],
                            [AttachedMessageId], [Resolved], [Reason], [ReportDate], [MessageType])
                            VALUES(@ReportingUserId, @ReportedUserId,
                            @AttachedMessageId, @Resolved, @Reason, @ReportDate, @MessageType)", report);
            }

            var reports = GetFromDatabase<MessageReport>("SELECT * FROM [MessageReport]");

            var response = await _httpClient.GetAsync($"{ApiUrl}?Resolved={false}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<MessageReportModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.All(content.Where(x => x.ReportingUserId == _authorizedUserId), 
                report => Assert.Equal(_authUsername, report.ReportingUserName));
            Assert.All(content.Where(x => x.ReportingUserId == user.Id),
                report => Assert.Equal(user.Name, report.ReportingUserName));
            Assert.Equivalent(reports.Where(x => !x.Resolved).Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authorize();

            var request = new AddMessageReportCommand
            {
                MessageId = 1,
                MessageType = MessageType.Direct,
                Reason = "res",
                ReportedUserId = 2,
                UserId = 3
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);

            var reports = GetFromDatabase<MessageReport>("SELECT * FROM [MessageReport]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(reports);
            Assert.Contains(reports, x => x.MessageType == request.MessageType && x.Reason == request.Reason 
                && x.ReportedUserId == request.ReportedUserId && x.ReportingUserId == request.UserId
                && x.AttachedMessageId == request.MessageId);
        }

        [Fact]
        public async Task PutAsync_Success()
        {
            await AuthorizeModerator();
            var reportToAdd = new MessageReport
            {
                AttachedMessageId = 1,
                MessageType = MessageType.Direct,
                Reason = "res1",
                ReportDate = DateTime.Now,
                ReportedUserId = 2,
                ReportingUserId = 3,
                Resolved = false
            };

            ExecuteQuery(@"INSERT INTO [MessageReport]([ReportingUserId], [ReportedUserId],
                            [AttachedMessageId], [Resolved], [Reason], [ReportDate], [MessageType])
                            VALUES(@ReportingUserId, @ReportedUserId,
                            @AttachedMessageId, @Resolved, @Reason, @ReportDate, @MessageType)", reportToAdd);

            var report = GetFromDatabase<MessageReport>("SELECT * FROM [MessageReport]").First();

            var request = new UpdateMessageReportCommand
            {
                Id = report.Id,
                Resolved = true
            };

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, request);

            var reports = GetFromDatabase<MessageReport>("SELECT * FROM [MessageReport]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(reports);
            Assert.Contains(reports, x => x.Resolved == request.Resolved);
        }
    }
}
