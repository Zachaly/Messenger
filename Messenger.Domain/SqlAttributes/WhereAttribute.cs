using Messenger.Domain.Enum;

namespace Messenger.Domain.SqlAttributes
{
    public class WhereAttribute : Attribute
    {
        public string Column { get; set; } = "";
        public string ContentWrapper { get; set; } = "";
        public WhereType Type { get; set; } = 0;
    }
}
