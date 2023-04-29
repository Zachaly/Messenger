using Messenger.Application;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;

namespace Messenger.Tests.Unit.Factory
{
    public class UserClaimFactoryTests
    {
        private readonly UserClaimFactory _factory;

        public UserClaimFactoryTests()
        {
            _factory = new UserClaimFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddUserClaimRequest { UserId = 1, Value = "val" };
            var claim = _factory.Create(request);

            Assert.Equal(request.UserId, claim.UserId);
            Assert.Equal(request.Value, claim.Value);
        }
        [Fact]
        public void CreateSystemClaimFromModel_CreatesProperClaim()
        {
            var model = new UserClaimModel { Value = "val" };

            var claim = _factory.CreateSystemClaimFromModel(model);

            Assert.Equal("Role", claim.Type);
            Assert.Equal(model.Value, claim.Value);
        }
    }
}
