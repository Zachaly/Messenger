namespace Messenger.Models.Friend.Request
{
    public class GetFriendsRequestsRequest
    {
        public long? ReceiverId { get; set; }
        public long? SenderId { get; set; }
    }
}
