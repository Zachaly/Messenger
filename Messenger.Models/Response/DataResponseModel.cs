namespace Messenger.Models.Response
{
    public class DataResponseModel<T> : ResponseModel
    {
        public T? Data { get; set; }
    }
}
