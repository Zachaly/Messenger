using Messenger.Models;

namespace Messenger.Application
{
    internal static class RequestExtentions
    {
        public static (int Index, int Size) GetIndexAndSize(this PagedRequest request)
            => (request.PageIndex ?? 0, request.PageSize ?? 10);
    }
}
