using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.ChatUser
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [User].[Id]=[ChatUser].[UserId]")]
    public class ChatUserModel
    {
        [SqlName(Name = "[ChatUser].[UserId]")]
        public long Id { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
