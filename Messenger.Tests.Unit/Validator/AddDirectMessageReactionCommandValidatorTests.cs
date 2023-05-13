using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class AddDirectMessageReactionCommandValidatorTests : ValidatorTest<AddDirectMessageReactionCommandValidator, AddDirectMessageReactionCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddDirectMessageReactionCommand
            {
                MessageId = 1,
                Reaction = "😀",
                ReceiverId = 2,
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void MessageIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddDirectMessageReactionCommand
            {
                MessageId = 0,
                Reaction = "😀",
                ReceiverId = 2,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReceiverIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddDirectMessageReactionCommand
            {
                MessageId = 1,
                Reaction = "😀",
                ReceiverId = 0,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReactionEmpty_DoesNotPassValidation()
        {
            var command = new AddDirectMessageReactionCommand
            {
                MessageId = 1,
                Reaction = "",
                ReceiverId = 0,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReactionExceedsMaxLength_DoesNotPassValidation()
        {
            var command = new AddDirectMessageReactionCommand
            {
                MessageId = 1,
                Reaction = "😀a",
                ReceiverId = 0,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
