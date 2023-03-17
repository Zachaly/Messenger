namespace Messenger.Domain.Entity
{
    public class DirectMessage
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public DateTime Created { get; set; }
        public bool Read { get; set; }
    }
}
