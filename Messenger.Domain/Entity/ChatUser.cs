namespace Messenger.Domain.Entity
{
    public class ChatUser
    {
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
