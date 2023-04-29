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
        {
            throw new NotImplementedException();
        }

        public Claim CreateSystemClaimFromModel(UserClaimModel model)
        {
            throw new NotImplementedException();
        }
    }
}
