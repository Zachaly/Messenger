﻿using Messenger.Models.DirectMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Hubs
{
    public interface IDirectMessageClient
    {
        Task GetMessage(DirectMessageModel message);
        Task ReadMessage(long messageId, bool read);
        Task ReactionUpdated(long messageId, string? reaction);
    }

    [Authorize(AuthenticationSchemes = "Websocket")]
    public class DirectMessageHub : Hub<IDirectMessageClient>
    {

    }
}
