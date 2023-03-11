using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Friend
{
    public class FriendRequestModel
    {
        public long Id { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string Name { get; set; }
        [SqlName(Name = "[User].[Id]")]
        public long UserId { get; set; }
    }
}
