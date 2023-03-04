using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Friend
{
    public class FriendListItem
    {
        public int Id { get; set; }
        [Join(Table = "User", Column = "Name", Statement = "JOIN ON [User].[Id]=[Friend].[User2Id]")]
        public string Name { get; set; }
    }
}
