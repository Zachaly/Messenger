using Messenger.Domain.Enum;
using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.DirectMessage.Request
{
    public class GetDirectMessagesRequest : PagedRequest
    {
        public long Id { get; set; }
        [Where(Column = "[DirectMessage].[SenderId]=@User1Id AND [DirectMessage].[ReceiverId]=@User2Id", Type = WhereType.OR)]
        public long User1Id { get; set; }
        [Where(Column = "[DirectMessage].[SenderId]=@User2Id AND [DirectMessage].[ReceiverId]=@User1Id", Type = WhereType.OR)]
        public long User2Id { get; set; }
    }
}
