namespace Messenger.Models.DirectMessageReaction.Request
{
    public class UpdateDirectMessageReactionRequest : PagedRequest
    {
        public long? MessageId { get; set; }
    }
}
