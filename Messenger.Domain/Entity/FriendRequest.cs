namespace Messenger.Domain.Entity
{
    public class FriendRequest
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public DateTime Created { get; set; }
    }
}
