namespace Messenger.Models.User.Request
{
    public class GetUserRequest : PagedRequest
    {
        public long? Id { get; set; }
    }
}
