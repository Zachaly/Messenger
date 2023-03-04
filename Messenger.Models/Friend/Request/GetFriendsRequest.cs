using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Friend.Request
{
    public class GetFriendsRequest
    {
        [Where(Column = "[FriendRequest].[SenderId]")]
        public long UserId { get; set; }
    }
}
