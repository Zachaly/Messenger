using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.ChatUser.Request
{
    public class UpdateChatUserRequest
    {
        [Where]
        public long UserId { get; set; }
        [Where]
        public long ChatId { get; set; }
        [SkipWhere]
        public bool? IsAdmin { get; set; }
    }
}
