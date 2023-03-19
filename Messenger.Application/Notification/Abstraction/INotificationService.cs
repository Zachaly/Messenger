using Messenger.Models.DirectMessage;
using Messenger.Models.Friend;

namespace Messenger.Application.Abstraction
{
    public interface INotificationService
    {
        Task SendDirectMessage(DirectMessageModel message, long senderId, long receiverId);
        Task ReadDirectMessage(long messageId, long senderId);
        Task SendFriendRequest(long requestId, long receiverId);
        Task SendFriendRequestResponse(FriendAcceptedResponse response);
    }
}
