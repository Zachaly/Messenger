namespace Messenger.Domain.SqlAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class JoinAttribute : Attribute
    {
        public string Statement { get; set; } = "";
        public bool Outside { get; set; } = false;
    }
}
