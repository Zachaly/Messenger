using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Friend.Request
{
    public class GetFriendsRequestsRequest
    {
        [Where(Column = "[FriendRequest].[Id]")]
        public long? Id { get; set; }
        [Join(Statement = "INNER JOIN [User] ON [User].[Id]=[FriendRequest].[SenderId]")]
        public long? ReceiverId { get; set; }
        [Join(Statement = "INNER JOIN [User] ON [User].[Id]=[FriendRequest].[ReceiverId]")]
        public long? SenderId { get; set; }
    }
}
