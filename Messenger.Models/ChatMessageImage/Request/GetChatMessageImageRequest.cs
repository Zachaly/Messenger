namespace Messenger.Models.ChatMessageImage.Request
{
    public class GetChatMessageImageRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? MessageId { get; set; }
    }
}
