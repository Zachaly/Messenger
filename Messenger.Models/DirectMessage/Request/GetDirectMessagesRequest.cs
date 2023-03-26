using Messenger.Domain.Enum;
using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.DirectMessage.Request
{
    public class GetDirectMessagesRequest : PagedRequest
    {
        [Where(Column = "[DirectMessage].[Id]=")]
        public long? Id { get; set; }
        [Where(Column = "[DirectMessage].[SenderId]=@User2Id AND [DirectMessage].[ReceiverId]=", Type = WhereType.OR)]
        public long? User1Id { get; set; }
        [Where(Column = "[DirectMessage].[SenderId]=@User1Id AND [DirectMessage].[ReceiverId]=", Type = WhereType.OR)]
        public long? User2Id { get; set; }

        [Where(Column = "[DirectMessage].[SenderId]=")]
        public long? SenderId { get; set; }
        [Where(Column = "[DirectMessage].[ReceiverId]=")]
        public long? ReceiverId { get; set; }

        [Where(Column = "[DirectMessage].[Read]=")]
        public bool? Read { get; set; }
    }
}
