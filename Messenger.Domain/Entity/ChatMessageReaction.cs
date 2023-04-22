namespace Messenger.Domain.Entity
{
    public class ChatMessageReaction
    {
        public long UserId { get; set; }
        public long MessageId { get; set; }
        public string Reaction { get; set; }
    }
}
