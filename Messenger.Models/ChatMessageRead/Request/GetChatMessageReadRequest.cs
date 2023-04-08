namespace Messenger.Models.ChatMessageRead.Request
{
    public class GetChatMessageReadRequest : PagedRequest
    {
        public long? UserId { get; set; }
        public long? ChatId { get; set; }
    }
}
