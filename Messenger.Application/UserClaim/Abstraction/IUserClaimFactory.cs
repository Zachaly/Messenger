using Messenger.Domain.Entity;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;
using System.Security.Claims;

namespace Messenger.Application.Abstraction
{
    public interface IUserClaimFactory
    {
        UserClaim Create(AddUserClaimRequest request);
        Claim CreateSystemClaimFromModel(UserClaimModel model);
    }
}
