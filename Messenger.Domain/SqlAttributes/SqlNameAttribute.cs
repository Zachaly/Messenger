namespace Messenger.Domain.SqlAttributes
{
    public class SqlNameAttribute : Attribute
    {
        public string Name { get; set; }
        public bool SkipName { get; set; } = false;
    }
}
