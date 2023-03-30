namespace Messenger.Domain.Entity
{
    public class DirectMessageImage
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public string FileName { get; set; }
    }
}
