using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class RespondToFriendRequestCommandValidatorTests : ValidatorTest<RespondToFriendRequestCommandValidator, RespondToFriendRequestCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new RespondToFriendRequestCommand
            {
                RequestId = 1
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void RequestIdBelowOne_DoesNotPassValidation()
        {
            var command = new RespondToFriendRequestCommand
            {
                RequestId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
