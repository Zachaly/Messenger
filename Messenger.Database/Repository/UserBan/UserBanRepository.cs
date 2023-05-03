using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.UserBan;
using Messenger.Models.UserBan.Request;

namespace Messenger.Database.Repository
{
    public class UserBanRepository : RepositoryBase<UserBan, UserBanModel, GetUserBanRequest>, IUserBanRepository
    {
        public UserBanRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "UserBan";
        }
    }
}
