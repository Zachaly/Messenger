using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Database.Migrations
{
    internal interface IMigrationManager
    {
        void CreateDatabase();
        void MigrateDatabase();
    }
}
