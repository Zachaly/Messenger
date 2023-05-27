namespace Messenger.Database.Migrations
{
    public interface IMigrationManager
    {
        void CreateDatabase();
        void MigrateDatabase();
    }
}
