using Dapper;
using FluentMigrator.Runner;
using Messenger.Database.Connection;
using Microsoft.Extensions.Configuration;

namespace Messenger.Database.Migrations
{
    internal class MigrationManager : IMigrationManager
    {
        private readonly IConfiguration _configuration;
        private readonly IMigrationRunner _migrationRunner;
        private readonly IConnectionFactory _connectionFactory;

        public MigrationManager(IConfiguration configuration, IMigrationRunner migrationRunner, IConnectionFactory connectionFactory)
        {
            _configuration = configuration;
            _migrationRunner = migrationRunner;
            _connectionFactory = connectionFactory;
        }
        public void CreateDatabase()
        {
            var query = "SELECT * FROM sys.databases WHERE name = @Name";

            using (var connection = _connectionFactory.GetMasterConnection())
            {
                var dbName = _configuration["DatabaseName"];
                var res = connection.Query(query, new { Name = dbName });
                if (res.Any())
                {
                    return;
                }

                var createDbQuery = $"CREATE DATABASE {dbName}";
                connection.Query(createDbQuery);
            }
        }

        public void MigrateDatabase()
        {
            _migrationRunner.ListMigrations();
            _migrationRunner.MigrateUp();
        }
    }
}
