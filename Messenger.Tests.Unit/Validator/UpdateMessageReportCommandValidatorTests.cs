using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class UpdateMessageReportCommandValidatorTests : ValidatorTest<UpdateMessageReportCommandValidator, UpdateMessageReportCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new UpdateMessageReportCommand
            {
                AttachedMessageId = 1,
                Id = 2,
                Reason = "reason",
                Resolved = true,
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void OnlyRequiredFields_PassesValidation()
        {
            var command = new UpdateMessageReportCommand
            {
                Id = 2,
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ReasonExceedsMaxLength_DoesNotPassValidation()
        {
            var command = new UpdateMessageReportCommand
            {
                Id = 2,
                Reason = new string('a', 101)
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void IdBelowOne_DoesNotPassValidation()
        {
            var command = new UpdateMessageReportCommand
            {
                Id = 0,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void AttachedMessageIdBelowOne_DoesNotPassValidation()
        {
            var command = new UpdateMessageReportCommand
            {
                Id = 2,
                AttachedMessageId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
