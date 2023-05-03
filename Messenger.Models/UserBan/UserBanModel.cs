using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.UserBan
{
    [Join(Statement = "INNER JOIN [User] ON [UserBan].[UserId]=[User].[Id]")]
    public class UserBanModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string UserName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
