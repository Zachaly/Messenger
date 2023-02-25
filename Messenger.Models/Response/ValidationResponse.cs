namespace Messenger.Models.Response
{
    public class ValidationResponse : ResponseModel
    {
        public IDictionary<string, IEnumerable<string>>? Errors { get; set; }
    }
}
