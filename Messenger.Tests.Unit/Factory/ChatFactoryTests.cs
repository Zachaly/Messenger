using Messenger.Application;
using Messenger.Models.Chat.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class ChatFactoryTests
    {
        private readonly ChatFactory _factory;

        public ChatFactoryTests()
        {
            _factory = new ChatFactory();
        }

        [Fact]
        public void Create_Creates_Proper_Entity()
        {
            var request = new AddChatRequest
            {
                Name = "name",
                UserId = 1,
            };

            var chat = _factory.Create(request);

            Assert.Equal(request.Name, chat.Name);
            Assert.Equal(request.UserId, chat.CreatorId);
        }
    }
}
