using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Hubs
{
    public interface IClaimClient
    {
        Task ClaimAdded(string claim);
    }

    [Authorize(AuthenticationSchemes = "Websocket")]
    public class ClaimHub : Hub<IClaimClient>
    {
        
    }
}
