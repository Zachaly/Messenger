namespace Messenger.Domain.Entity
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public string Content { get; set; }
        public long SenderId { get; set; }
        public DateTime Created { get; set; }
    }
}
