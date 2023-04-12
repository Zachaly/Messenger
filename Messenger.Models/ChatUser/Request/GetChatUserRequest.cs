using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.ChatUser.Request
{
    public class GetChatUserRequest : PagedRequest
    {
        [Where(Column = "[ChatUser].[UserId]=")]
        public long? UserId { get; set; }
        [Where(Column = "[ChatUser].[ChatId]=")]
        public long? ChatId { get; set; }
    }
}
