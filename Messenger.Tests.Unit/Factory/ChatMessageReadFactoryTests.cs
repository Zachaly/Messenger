using Messenger.Application;
using Messenger.Models.ChatMessageRead.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class ChatMessageReadFactoryTests
    {
        private readonly ChatMessageReadFactory _factory;

        public ChatMessageReadFactoryTests()
        {
            _factory = new ChatMessageReadFactory();
        }

        [Fact]
        public void Create_Creates_Proper_Entity()
        {
            var request = new AddChatMessageReadRequest { MessageId = 1, UserId = 2 };

            var read = _factory.Create(request);
            
            Assert.Equal(request.UserId, read.UserId);
            Assert.Equal(request.MessageId, read.MessageId);
        }
    }
}
