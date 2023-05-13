using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class AddChatMessageReadCommandValidatorTests : ValidatorTest<AddChatMessageReadCommandValidator, AddChatMessageReadCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddChatMessageReadCommand
            {
                ChatId = 1,
                MessageId = 2,
                UserId = 3
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ChatIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageReadCommand
            {
                ChatId = 0,
                MessageId = 2,
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void MessageIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageReadCommand
            {
                ChatId = 1,
                MessageId = 0,
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void UserIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageReadCommand
            {
                ChatId = 1,
                MessageId = 2,
                UserId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
