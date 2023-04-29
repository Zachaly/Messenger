using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;
using System.Security.Claims;

namespace Messenger.Application
{
    public class UserClaimFactory : IUserClaimFactory
    {
        public UserClaim Create(AddUserClaimRequest request)
            => new UserClaim
            {
                UserId = request.UserId,
                Value = request.Value,
            };

        public Claim CreateSystemClaimFromModel(UserClaimModel model)
            => new Claim("Role", model.Value);
    }
}
