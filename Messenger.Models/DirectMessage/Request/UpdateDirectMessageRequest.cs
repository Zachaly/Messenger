namespace Messenger.Models.DirectMessage.Request
{
    public class UpdateDirectMessageRequest
    {
        public long Id { get; set; }
        public bool? Read { get; set; }
    }
}
