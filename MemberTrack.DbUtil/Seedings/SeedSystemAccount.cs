using MemberTrack.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace MemberTrack.DbUtil.Seedings
{
    class SeedSystemAccount : SeedBase
    {
        const string userTableName = "dbo.[User]";

        public SeedSystemAccount(DatabaseContext databaseContext, bool forceReseeding)
            : base(databaseContext, forceReseeding)
        {
        }

        protected override void ClearData()
        {
            foreach (var userAccount in SystemAccountHelper.SystemAccounts)
            {
                _databaseContext.Database.ExecuteSqlCommand(
                    $"DELETE FROM {userTableName} WHERE Email = '{userAccount.Email}'");
            }
        }

        protected override void PopulateData()
        {
            var seeded = _databaseContext.Users.Any(x => SystemAccountHelper.IsSystemAccount(x.Id));

            if (seeded)
            {
                Console.WriteLine("System accounts have already been seeded.  Use the -f command line option, if your intent was to repopulate the system accounts.");
                return;
            }

            foreach (var userAccount in SystemAccountHelper.SystemAccounts)
            {
                var query = $@"SET IDENTITY_INSERT {userTableName} ON
								INSERT INTO {userTableName}
									(Id, DisplayName, Role, Password, Email) 
								VALUES({userAccount.Id}, '{userAccount.DisplayName}', 
                    {(int)userAccount.Role}, '{userAccount.Password}', '{userAccount.Email}')
								SET IDENTITY_INSERT {userTableName} OFF";

                _databaseContext.Database.ExecuteSqlCommand(query);
            }

        }
    }
}
