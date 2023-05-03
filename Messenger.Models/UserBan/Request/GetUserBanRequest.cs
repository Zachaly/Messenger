using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.UserBan.Request
{
    public class GetUserBanRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        [Where(Column = "[UserBan].[Start]>=")]
        public DateTime? MinimalStart { get; set; }
        [Where(Column = "[UserBan].[End]<=")]
        public DateTime? MaximalEnd { get; set; }
    }
}
