using Messenger.Application;
using Messenger.Models.DirectMessage.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class DirectMessageFactoryTests
    {
        private readonly DirectMessageFactory _factory;

        public DirectMessageFactoryTests()
        {
            _factory = new DirectMessageFactory();
        }

        [Fact]
        public void Create_Creates_Proper_Entity()
        {
            var request = new AddDirectMessageRequest
            {
                Content = "content",
                ReceiverId = 1,
                SenderId = 2
            };

            var message = _factory.Create(request);

            Assert.Equal(request.Content, message.Content);
            Assert.Equal(request.ReceiverId, message.ReceiverId);
            Assert.Equal(request.SenderId, message.SenderId);
            Assert.NotEqual(default, message.Created);
        }
    }
}
