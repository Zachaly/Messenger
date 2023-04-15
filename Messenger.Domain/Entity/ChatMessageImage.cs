namespace Messenger.Domain.Entity
{
    public class ChatMessageImage
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public long MessageId { get; set; }
    }
}
