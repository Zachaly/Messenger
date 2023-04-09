namespace Messenger.Models.ChatUser.Request
{
    public class UpdateChatUserRequest
    {
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
