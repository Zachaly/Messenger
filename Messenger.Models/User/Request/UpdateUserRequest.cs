using Messenger.Domain.SqlAttributes;

namespace Messenger.Models.User.Request
{
    public class UpdateUserRequest
    {
        [Where]
        public long Id { get; set; }
        public string? ProfileImage { get; set; }
        public string? Name { get; set; }
    }
}
