using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Hubs
{
    public static class HubExtension
    {
        public static long GetUserId(this Hub @this)
            => long.Parse(@this.Context.UserIdentifier!);
    }
}
