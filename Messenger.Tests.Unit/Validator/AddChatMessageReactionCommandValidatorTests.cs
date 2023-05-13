using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class AddChatMessageReactionCommandValidatorTests : ValidatorTest<AddChatMessageReactionCommandValidator, AddChatMessageReactionCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddChatMessageReactionCommand
            {
                ChatId = 1,
                MessageId = 2,
                Reaction = "😀",
                UserId = 3
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ChatIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageReactionCommand
            {
                ChatId = 0,
                MessageId = 2,
                Reaction = "😀",
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void MessageIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageReactionCommand
            {
                ChatId = 1,
                MessageId = 0,
                Reaction = "😀",
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void UserIdIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddChatMessageReactionCommand
            {
                ChatId = 1,
                MessageId = 2,
                Reaction = "😀",
                UserId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReactionEmpty_DoesNotPassValidation()
        {
            var command = new AddChatMessageReactionCommand
            {
                ChatId = 1,
                MessageId = 2,
                Reaction = "",
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReactionExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new AddChatMessageReactionCommand
            {
                ChatId = 1,
                MessageId = 2,
                Reaction = "😀a",
                UserId = 3
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
