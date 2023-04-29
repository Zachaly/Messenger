namespace Messenger.Models.User
{
    public class LoginResponse
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string AuthToken { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
