namespace Messenger.Models.User.Request
{
    public class UpdateUserRequest
    {
        public long Id { get; set; }
        public string? ProfileImage { get; set; }
        public string? Name { get; set; }
    }
}
