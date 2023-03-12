namespace Messenger.Models.Friend
{
    public class FriendAcceptedResponse
    {
        public long SenderId { get; set; }
        public bool Accepted { get; set; }
        public string Name { get; set; }
    }
}
