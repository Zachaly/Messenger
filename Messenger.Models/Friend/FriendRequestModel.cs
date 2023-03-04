using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Friend
{
    public class FriendRequestModel
    {
        public long Id { get; set; }
        [Join(Table = "User", Column = "Name", Statement = "JOIN [User] ON [User].[Id]=[FriendRequest].[ReceiverId] OR [User].[Id]=[FriendRequest].[SenderId]")]
        public string Name { get; set; }
        public long ReceiverId { get; set; }
    }
}
