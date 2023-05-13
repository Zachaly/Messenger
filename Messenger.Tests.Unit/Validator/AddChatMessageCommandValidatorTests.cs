using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class AddChatMessageCommandValidatorTests : ValidatorTest<AddChatMessageCommandValidator, AddChatMessageCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddChatMessageCommand
            {
                ChatId = 1,
                Content = "con",
                SenderId = 1
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ChatIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageCommand
            {
                ChatId = 0,
                Content = "con",
                SenderId = 1
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void SenderIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageCommand
            {
                ChatId = 1,
                Content = "con",
                SenderId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ContentExceedsMaxLength_DoesNotPassValidation()
        {
            var command = new AddChatMessageCommand
            {
                ChatId = 1,
                Content = new string('a', 501),
                SenderId = 1
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ContentEmpty_DoesNotPassValidation()
        {
            var command = new AddChatMessageCommand
            {
                ChatId = 1,
                Content = "",
                SenderId = 1
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
