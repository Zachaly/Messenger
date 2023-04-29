using Messenger.Database.Repository.Abstraction;
using Messenger.Domain.Entity;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;

namespace Messenger.Database.Repository
{
    public interface IUserClaimRepository : IKeylessRepository<UserClaim, UserClaimModel, GetUserClaimRequest>
    {
        Task DeleteAsync(long userId, string value);
    }
}
