namespace Messenger.Models.ChatMessage.Request
{
    public class AddChatMessageRequest
    {
        public long ChatId { get; set; }    
        public string Content { get; set; }
        public long SenderId { get; set; }
    }
}
