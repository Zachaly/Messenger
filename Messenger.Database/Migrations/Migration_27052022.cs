using FluentMigrator;

namespace Messenger.Database.Migrations
{
    [Migration(27052022)]
    public class Migration_27052022 : Migration
    {
        public override void Down()
        {
            Delete.Table("Chat");
            Delete.Table("ChatMessage");
            Delete.Table("ChatMessageImage");
            Delete.Table("ChatMessageReaction");
            Delete.Table("ChatMessageRead");
            Delete.Table("ChatUser");
            Delete.Table("DirectMessage");
            Delete.Table("DirectMessageImage");
            Delete.Table("DirectMessageReaction");
            Delete.Table("Friend");
            Delete.Table("FriendRequest");
            Delete.Table("MessageReport");
            Delete.Table("User");
            Delete.Table("UserBan");
            Delete.Table("UserClaim");
        }

        public override void Up()
        {
            Create.Table("Chat")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("CreatorId").AsInt64().NotNullable();

            Create.Table("ChatMessage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("ChatId").AsInt64().NotNullable()
                .WithColumn("Content").AsString(500).NotNullable()
                .WithColumn("SenderId").AsInt64().NotNullable()
                .WithColumn("Created").AsDateTime2().NotNullable();

            Create.Table("ChatMessageImage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("FileName").AsString().NotNullable()
                .WithColumn("MessageId").AsInt64().NotNullable();

            Create.Table("ChatMessageReaction")
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("MessageId").AsInt64().NotNullable()
                .WithColumn("Reaction").AsString(2);

            Create.Table("ChatMessageRead")
                .WithColumn("MessageId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable();
                

            Create.Table("ChatUser")
                .WithColumn("ChatId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsAdmin").AsBoolean().NotNullable();

            Create.Table("DirectMessage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Content").AsString(500).NotNullable()
                .WithColumn("SenderId").AsInt64().NotNullable()
                .WithColumn("ReceiverId").AsInt64().NotNullable()
                .WithColumn("Created").AsDateTime2().NotNullable()
                .WithColumn("Read").AsBoolean().NotNullable();

            Create.Table("DirectMessageImage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("MessageId").AsInt64().NotNullable()
                .WithColumn("FileName").AsString().NotNullable();

            Create.Table("DirectMessageReaction")
                .WithColumn("MessageId").AsInt64().NotNullable()
                .WithColumn("Reaction").AsString(2).NotNullable();

            Create.Table("Friend")
                .WithColumn("User1Id").AsInt64().NotNullable()
                .WithColumn("User2Id").AsInt64().NotNullable();

            Create.Table("FriendRequest")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("SenderId").AsInt64().NotNullable()
                .WithColumn("ReceiverId").AsInt64().NotNullable()
                .WithColumn("Created").AsDateTime2().NotNullable();

            Create.Table("MessageReport")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("ReportingUserId").AsInt64().NotNullable()
                .WithColumn("ReportedUserId").AsInt64().NotNullable()
                .WithColumn("Resolved").AsBoolean().NotNullable()
                .WithColumn("AttachedMessageId").AsInt64().NotNullable()
                .WithColumn("Reason").AsString(100).NotNullable()
                .WithColumn("ReportDate").AsDateTime2().NotNullable()
                .WithColumn("MessageType").AsInt16().NotNullable();

            Create.Table("User")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Login").AsString(100).NotNullable()
                .WithColumn("PasswordHash").AsString().NotNullable()
                .WithColumn("ProfileImage").AsString().Nullable();

            Create.Table("UserBan")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Start").AsDateTime2().NotNullable()
                .WithColumn("End").AsDateTime2().NotNullable();

            Create.Table("UserClaim")
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Value").AsString().NotNullable();
        }
    }
}
