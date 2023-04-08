namespace Messenger.Models.ChatUser.Request
{
    public class GetChatUserRequest : PagedRequest
    {
        public long? UserId { get; set; }
        public long? ChatId { get; set; }
    }
}
