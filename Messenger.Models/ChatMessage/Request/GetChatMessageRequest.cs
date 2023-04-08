namespace Messenger.Models.ChatMessage.Request
{
    public class GetChatMessageRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? ChatId { get; set; }
    }
}
