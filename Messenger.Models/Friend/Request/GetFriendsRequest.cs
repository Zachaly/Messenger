using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Friend.Request
{
    public class GetFriendsRequest : PagedRequest
    {
        [Where(Column = "[Friend].[User1Id]=")]
        public long UserId { get; set; }
    }
}
