using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Friend
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [User].[Id]=[Friend].[User2Id]")]
    public class FriendListItem
    {
        [SqlName(Name = "[User].[Id]")]
        public long Id { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string Name { get; set; }
    }
}
