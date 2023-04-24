using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.User.Request
{
    public class GetUserRequest : PagedRequest
    {
        public long? Id { get; set; }
        [Where(Column = "[User].[Name] LIKE ", ContentWrapper = "%")]
        public string? SearchName { get; set; }
    }
}
