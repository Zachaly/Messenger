namespace Messenger.Domain.Entity
{
    public class UserBan
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
