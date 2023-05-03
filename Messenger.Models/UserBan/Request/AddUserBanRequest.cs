namespace Messenger.Models.UserBan.Request
{
    public class AddUserBanRequest
    {
        public long UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
