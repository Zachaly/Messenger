namespace Messenger.Models.ChatMessageReaction.Request
{
    public class AddChatMessageReactionRequest
    {
        public long UserId { get; set; }
        public string Reaction { get; set; }
        public long MessageId { get; set; }
    }
}
