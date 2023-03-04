namespace Messenger.Domain.SqlAttributes
{
    public class JoinAttribute : Attribute
    {
        public string Statement { get; set; } = "";
    }
}
