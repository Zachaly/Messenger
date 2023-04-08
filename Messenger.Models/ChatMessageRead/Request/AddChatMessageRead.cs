namespace Messenger.Models.ChatMessageRead.Request
{
    public class AddChatMessageRead
    {
        public long MessageId { get; set; }
        public long UserId { get; set; }
    }
}
