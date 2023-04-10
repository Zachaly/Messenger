using MediatR;
using Messenger.Application.Command;
using Messenger.Models.Friend;
using Messenger.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Hubs
{
    public static class ConnectedUsers
    {
        public static List<long> Ids { get; } = new List<long>();
    }

    public interface StatusClient
    {
        Task FriendConnected(UserModel friend);
        Task FriendConnected(FriendListItem firend);
        Task FriendDisconnected(long friendId);
    }

    [Authorize(AuthenticationSchemes = "Websocket")]
    public class StatusHub : Hub<StatusClient>
    {
        private readonly IMediator _mediator;

        public StatusHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        private async Task<IEnumerable<FriendListItem>> GetFriends(long userId)
        {
            var friendCount = await _mediator.Send(new GetFriendCountQuery { UserId = userId });
            return (await _mediator.Send(new GetFriendsQuery { UserId = userId, PageSize = friendCount }));
        }

        public override async Task OnConnectedAsync()
        {
            var id = this.GetUserId();

            var friends = await GetFriends(id);
            var user = await _mediator.Send(new GetUserByIdQuery { UserId = id });

            await Clients.Users(friends.Select(friend => friend.Id.ToString())).FriendConnected(user);

            if (!ConnectedUsers.Ids.Contains(id))
            {
                ConnectedUsers.Ids.Add(id);
            }
            
            base.OnConnectedAsync();
        }

        public async Task<IEnumerable<FriendListItem>> GetOnlineFriends()
        {
            var id = this.GetUserId();

            var onlineFriends = (await GetFriends(id)).Where(friend => ConnectedUsers.Ids.Contains(friend.Id));
            
            return onlineFriends;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var id = long.Parse(Context.UserIdentifier);

            var friendCount = await _mediator.Send(new GetFriendCountQuery { UserId = id });
            var friends = await _mediator.Send(new GetFriendsQuery { UserId = id, PageSize = friendCount });

            await Clients.Users(friends.Select(friend => friend.Id.ToString())).FriendDisconnected(id);

            ConnectedUsers.Ids.Remove(id);

            base.OnDisconnectedAsync(exception);
        }
    }
}
