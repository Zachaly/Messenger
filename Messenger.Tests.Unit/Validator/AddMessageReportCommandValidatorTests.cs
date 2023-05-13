using Messenger.Application.Command;
using Messenger.Application.Validator;
using System.Formats.Asn1;

namespace Messenger.Tests.Unit.Validator
{
    public class AddMessageReportCommandValidatorTests : ValidatorTest<AddMessageReportCommandValidator, AddMessageReportCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddMessageReportCommand
            {
                MessageId = 1,
                Reason = "reason",
                ReportedUserId = 2,
                UserId = 3
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void MessageIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddMessageReportCommand
            {
                MessageId = 0,
                Reason = "reason",
                ReportedUserId = 2,
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReportedUserIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddMessageReportCommand
            {
                MessageId = 1,
                Reason = "reason",
                ReportedUserId = 0,
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void UserIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddMessageReportCommand
            {
                MessageId = 1,
                Reason = "reason",
                ReportedUserId = 2,
                UserId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReasonEmpty_DoesNotPassValidation()
        {
            var command = new AddMessageReportCommand
            {
                MessageId = 1,
                Reason = "",
                ReportedUserId = 2,
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReasonExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new AddMessageReportCommand
            {
                MessageId = 1,
                Reason = new string('a', 101),
                ReportedUserId = 2,
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
