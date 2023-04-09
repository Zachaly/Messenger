using Messenger.Application;
using Messenger.Models.ChatMessage.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class ChatMessageFactoryTests
    {
        private readonly ChatMessageFactory _factory;

        public ChatMessageFactoryTests()
        {
            _factory = new ChatMessageFactory();
        }

        [Fact]
        public void Create_Creates_Proper_Entity()
        {
            var request = new AddChatMessageRequest
            {
                ChatId = 1,
                Content = "content",
                SenderId = 2
            };

            var message = _factory.Create(request);

            Assert.Equal(request.ChatId, message.ChatId);
            Assert.Equal(request.Content, message.Content);
            Assert.Equal(request.SenderId, message.SenderId);
            Assert.NotEqual(default, message.Created);
        }
    }
}
