using Messenger.Application;
using Messenger.Models.UserBan.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Factory
{
    public class UserBanFactoryTests
    {
        private readonly UserBanFactory _factory;

        public UserBanFactoryTests()
        {
            _factory = new UserBanFactory();
        }

        [Fact]
        public async Task Create_CreatesProperEntity()
        {
            var request = new AddUserBanRequest
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(2),
                UserId = 1
            };

            var ban = _factory.Create(request);

            Assert.Equal(request.Start, ban.Start);
            Assert.Equal(request.End, ban.End);
            Assert.Equal(request.UserId, ban.UserId);
        }
    }
}
