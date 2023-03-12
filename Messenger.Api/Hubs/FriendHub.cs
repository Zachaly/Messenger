using Messenger.Models.Friend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Hubs
{
    public interface IFriendClient
    {
        Task GetRequest(long requestId);
        Task GetRequestResponse(FriendAcceptedResponse response);
    }

    [Authorize(AuthenticationSchemes = "Websocket")]
    public class FriendHub : Hub<IFriendClient>
    {
        
    }
}
