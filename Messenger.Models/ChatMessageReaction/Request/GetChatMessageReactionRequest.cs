namespace Messenger.Models.ChatMessageReaction.Request
{
    public class GetChatMessageReactionRequest : PagedRequest
    {
        public long? MessageId { get; set; }
        public long? UserId { get; set; }
        public string? Reaction { get; set; }
    }
}
