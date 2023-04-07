namespace Messenger.Models.DirectMessageReaction.Request
{
    public class AddDirectMessageReactionRequest
    {
        public long MessageId { get; set; }
        public string Reaction { get; set; }
    }
}
