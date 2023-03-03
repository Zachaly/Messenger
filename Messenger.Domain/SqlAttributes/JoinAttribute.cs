namespace Messenger.Domain.SqlAttributes
{
    public class JoinAttribute : Attribute
    {
        public string Statement { get; set; } = "";
        public string Table { get; set; } = "";
        public string Column { get; set; } = "";
    }
}
