namespace Messenger.Models.ChatMessageRead.Request
{
    public class AddChatMessageReadRequest
    {
        public long MessageId { get; set; }
        public long UserId { get; set; }
    }
}
