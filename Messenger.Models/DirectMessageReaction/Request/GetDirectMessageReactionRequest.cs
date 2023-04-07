namespace Messenger.Models.DirectMessageReaction.Request
{
    public class GetDirectMessageReactionRequest : PagedRequest
    {
        public long? MessageId { get; set; }
    }
}
