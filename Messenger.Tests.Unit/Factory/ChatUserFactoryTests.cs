using Messenger.Application;
using Messenger.Models.ChatUser.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class ChatUserFactoryTests
    {
        private readonly ChatUserFactory _factory;

        public ChatUserFactoryTests()
        {
            _factory = new ChatUserFactory();
        }

        [Fact]
        public void Create_WithRequest_Creates_Proper_Entity()
        {
            var request = new AddChatUserRequest { ChatId = 1, UserId = 2 };

            var user = _factory.Create(request);

            Assert.Equal(request.UserId, user.UserId);
            Assert.Equal(request.ChatId, user.ChatId);
        }

        [Fact]
        public void Create_WithIds_Creates_Proper_Entity()
        {
            const long ChatId = 1;
            const long UserId = 2;

            var user = _factory.Create(ChatId, UserId);

            Assert.Equal(ChatId, user.ChatId);
            Assert.Equal(UserId, user.UserId);
            Assert.True(user.IsAdmin);
        }
    }
}
