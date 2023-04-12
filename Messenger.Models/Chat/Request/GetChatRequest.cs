using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.Chat.Request
{
    public class GetChatRequest : PagedRequest
    {
        [Where(Column = "[ChatUser].[UserId]=")]
        [Join(Statement = "LEFT OUTER JOIN [ChatUser] ON [ChatUser].[ChatId]=[Chat].[Id]")]
        public long? UserId { get; set; }
        [Where(Column = "[Chat].[Id]=")]
        public long? Id { get; set; }
    }
}
