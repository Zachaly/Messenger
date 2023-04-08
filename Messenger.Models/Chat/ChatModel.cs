using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Chat
{
    [Join(Statement = "LEFT OUTER JOIN [User] ON [User].[Id]=[Chat].[CreatorId]")]
    public class ChatModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CreatorId { get; set; }
        [SqlName(Name = "[User.Name]")]
        public string CreatorName { get; set; }
    }
}
