using Messenger.Domain.Entity;
using Messenger.Models.ChatMessage;
using System.ComponentModel;

namespace Messenger.Tests.Integration
{
    internal static class FakeDataFactory
    {
        public static IEnumerable<User> CreateUsers(int count)
        {
            var users = new List<User>();

            for(int i = 1; i <= count; i++)
            {
                users.Add(new User { Id = i, Login = $"log{i}", Name = $"name{i}", PasswordHash = $"hash{i}", ProfileImage = $"profile {i}" });
            }

            return users;
        }
        
        public static IEnumerable<Friend> CreateFriends(long userId, IEnumerable<long> friendIds)
        {
            return friendIds.Select(id => new Friend { User1Id = userId, User2Id = id });
        }

        public static IEnumerable<Friend> CreateFriends(IEnumerable<long> friendIds, long userId)
        {
            return friendIds.Select(id => new Friend { User1Id = id, User2Id = userId });
        }

        public static IEnumerable<FriendRequest> CreateFriendRequests(long receiverId, IEnumerable<long> senderIds)
            => senderIds.Select(id => new FriendRequest { Created = DateTime.Now, SenderId = id, ReceiverId = receiverId });

        public static IEnumerable<FriendRequest> CreateFriendRequests(IEnumerable<long> receiverIds, long senderId)
            => receiverIds.Select(id => new FriendRequest { Created = DateTime.Now, SenderId = senderId, ReceiverId = id });

        public static IEnumerable<DirectMessage> CreateMessages(long senderId, long receiverId, int count)
        {
            var messages = new List<DirectMessage>();

            for(int i = 0; i < count; i++)
            {
                messages.Add(new DirectMessage { Content = $"{senderId}-{receiverId}", Created = DateTime.Now,
                    Read = false, ReceiverId = receiverId, SenderId = senderId });
            }

            return messages;
        }

        public static IEnumerable<DirectMessageImage> CreateMessageImages(long messageId, int count)
        {
            var images = new List<DirectMessageImage>();

            for(int i = 0; i < count; i++)
            {
                images.Add(new DirectMessageImage { MessageId = messageId, FileName = $"image {i}" });
            }

            return images;
        }

        public static IEnumerable<ChatMessage> CreateChatMessages(long chatId, IEnumerable<long> senderIds)
            => senderIds.Select(id => new ChatMessage { ChatId = chatId, Content = $"Sender{id}{chatId}", Created = DateTime.Now, SenderId = id });

        public static IEnumerable<ChatMessageRead> CreateChatMessageReads(long messageId, IEnumerable<long> readerIds)
            => readerIds.Select(id => new ChatMessageRead { MessageId = messageId, UserId = id });

        public static IEnumerable<ChatUser> CreateChatUsers(long chatId, IEnumerable<long> userIds)
            => userIds.Select(id => new ChatUser { ChatId = chatId, UserId = id });

        public static IEnumerable<Chat> CreateChats(long creatorId, int count)
        {
            var chats = new List<Chat>();

            for(int i = 0; i < count; i++)
            {
                chats.Add(new Chat { CreatorId = creatorId, Name = $"Chat{i}" });
            }

            return chats;
        }

        public static IEnumerable<ChatMessageImage> CreateChatMessageImages(long messageId, int count)
        {
            var images = new List<ChatMessageImage>();
            for(int i = 0; i < count; i++)
            {
                images.Add(new ChatMessageImage { FileName = $"img{i}", MessageId = messageId });
            }

            return images;
        }
    }
}
