using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.UserClaim;
using Messenger.Models.UserClaim.Request;

namespace Messenger.Database.Repository
{
    public class UserClaimRepository : KeylessRepositoryBase<UserClaim, UserClaimModel, GetUserClaimRequest>, IUserClaimRepository
    {
        public UserClaimRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "UserClaim";
            DefaultOrderBy = "[UserId]";
        }

        public Task DeleteAsync(long userId, string value)
        {
            throw new NotImplementedException();
        }
    }
}
