using Dapper;
using Messenger.Database.Connection;
using Messenger.Domain.Entity;
using Moq;
using System.Data.SqlClient;

namespace Messenger.Tests.Integration.Database
{
    public abstract class DatabaseTest : IDisposable
    {
        protected string _connectionString = "Server=localhost;Database=MessengerTest;Trusted_Connection=True;";
        protected List<string> _teardownQueries = new List<string>();
        protected readonly IConnectionFactory _connectionFactory;

        protected DatabaseTest()
        {
            var connectionFactory = new Mock<IConnectionFactory>();
            connectionFactory.Setup(x => x.GetConnection())
                .Returns(new SqlConnection(_connectionString));

            _connectionFactory = connectionFactory.Object;
        }

        public void Dispose()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var query in _teardownQueries)
                {
                    connection.Query(query);
                }
            }
        }

        protected async Task<IEnumerable<T>> GetAllFromDatabase<T>(string table)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<T>($"SELECT * FROM [{table}]");
            }
        }

        protected async Task InsertChatsToDatabase(IEnumerable<Chat> chats)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var chat in chats)
                {
                    await connection.QueryAsync("INSERT INTO [Chat]([CreatorId], [Name]) VALUES(@CreatorId, @Name)", chat);
                }
            }
        }

        protected async Task InsertChatMessagesToDatabase(IEnumerable<ChatMessage> messages)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var msg in messages)
                {
                    await connection.QueryAsync("INSERT INTO [ChatMessage]([SenderId], [ChatId], [Created], [Content])" +
                        " VALUES (@SenderId, @ChatId, @Created, @Content)", msg);
                }
            }
        }

        protected async Task InsertChatMessageReadsToDatabase(IEnumerable<ChatMessageRead> messages)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var read in messages)
                {
                    await connection.QueryAsync("INSERT INTO [ChatMessageRead]([UserId], [MessageId]) VALUES (@UserId, @MessageId)", read);
                }
            }
        }

        protected async Task InsertChatUsersToDatabase(IEnumerable<ChatUser> users)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var user in users)
                {
                    await connection.QueryAsync("INSERT INTO [ChatUser]([ChatId], [UserId], [IsAdmin]) VALUES (@ChatId, @UserId, @IsAdmin)", user);
                }
            }
        }

        protected async Task InsertUsersToDatabase(IEnumerable<User> users)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var user in users)
                {
                    await connection.QueryAsync("INSERT INTO [User]([Login], [Name], [PasswordHash]) VALUES(@Login, @Name, @PasswordHash)", user);
                }
            }
        }

        protected async Task InsertImagesToDatabase(IEnumerable<DirectMessageImage> images)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var image in images)
                {
                    await connection.ExecuteAsync("INSERT INTO [DirectMessageImage]([MessageId], [FileName]) VALUES(@MessageId, @FileName)", image);
                }
            }
        }

        protected async Task InsertImagesToDatabase(IEnumerable<ChatMessageImage> images)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var image in images)
                {
                    await connection.ExecuteAsync("INSERT INTO [ChatMessageImage]([MessageId], [FileName]) VALUES(@MessageId, @FileName)", image);
                }
            }
        }

        protected async Task InsertFriendsToDatabase(IEnumerable<Friend> friends)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var friend in friends)
                {
                    await connection.QueryAsync("INSERT INTO [Friend]([User1Id], [User2Id]) VALUES(@User1Id, @User2Id)", friend);
                }
            }
        }

        protected async Task InsertDirectMessageReactionToDatabase(DirectMessageReaction reaction)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync("INSERT INTO [DirectMessageReaction]([MessageId], [Reaction]) VALUES(@MessageId, @Reaction)", reaction);
            }
        }

        protected async Task InsertChatMessageReactionToDatabase(ChatMessageReaction reaction)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync("INSERT INTO [ChatMessageReaction]([MessageId], [UserId], [Reaction]) VALUES (@MessageId, @UserId, @Reaction)",
                    reaction);
            }
        }
    }
}
