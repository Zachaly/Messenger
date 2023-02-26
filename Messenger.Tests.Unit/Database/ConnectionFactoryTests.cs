using Messenger.Database.Connection;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Messenger.Tests.Unit.Database
{
    public class ConnectionFactoryTests
    {
        [Fact]
        public void GetConnection_Creates_Proper_Connection()
        {
            var config = new Mock<IConfiguration>();
            config.Setup(x => x["ConnectionString"]).Returns("Server=localhost");
            var factory = new ConnectionFactory(config.Object);

            var connection = factory.GetConnection();
            Assert.NotNull(connection);
        }
    }
}
