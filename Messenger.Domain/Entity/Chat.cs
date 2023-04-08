namespace Messenger.Domain.Entity
{
    public class Chat
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CreatorId { get; set; }
    }
}
