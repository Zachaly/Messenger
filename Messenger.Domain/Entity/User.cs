namespace Messenger.Domain.Entity
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string? ProfileImage { get; set; }
    }
}
