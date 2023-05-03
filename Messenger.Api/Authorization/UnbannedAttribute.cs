namespace Microsoft.AspNetCore.Authorization
{
    public class UnbannedAttribute : AuthorizeAttribute
    {
        public UnbannedAttribute()
        {
            Policy = "Unbanned";
        }
    }
}
