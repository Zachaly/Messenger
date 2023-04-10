using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.ChatMessage.Request
{
    public class GetChatMessageRequest : PagedRequest
    {
        [Where(Column = "[ChatMessage].[Id]=")]
        public long? Id { get; set; }
        public long? ChatId { get; set; }
    }
}
