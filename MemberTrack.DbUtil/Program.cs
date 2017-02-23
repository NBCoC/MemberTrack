using System;
using System.Threading.Tasks;
using MemberTrack.Common;
using MemberTrack.Data;
using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.DbUtil
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("START");

            try
            {
                var arguments = new Arguments().Parse(args);

                Console.WriteLine("Datasource: {0}", arguments.Datasource);
                Console.WriteLine("Catalog: {0}", arguments.Catalog);
                if (arguments.ForceReseeding)
                {
                    Console.WriteLine("Forcing the reseeding of the system accounts", arguments.Catalog);
                }

                using (var dbContext = new DatabaseContextFactory().Create(arguments.Datasource, arguments.Catalog))
                {
                    Console.WriteLine("Creating system account...");
                    SeedSystemAccount(dbContext, arguments.ForceReseeding).Wait();
                }

                Console.WriteLine("Database seeded.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToDetail());
            }
            finally
            {
                Console.WriteLine("END");
            }
        }

        private static async Task SeedSystemAccount(DatabaseContext context, bool forced)
        {
            const string UserTableName = "dbo.[User]";

            try
            {
                var seeded = await context.Users.AnyAsync(x => SystemAccountHelper.IsSystemAccount(x.Id));

                if (seeded && !forced) return;

                if (forced)
                {
                    context.Database.ExecuteSqlCommand($"TRUNCATE TABLE {UserTableName}");
                }

                foreach (User userAccount in SystemAccountHelper.SystemAccounts)
                {
                    var query = $@"SET IDENTITY_INSERT {UserTableName} ON
								INSERT INTO {UserTableName}
									(Id, DisplayName, Role, Password, Email) 
								VALUES({userAccount.Id}, '{userAccount.DisplayName}', 
                    {(int)userAccount.Role}, '{userAccount.Password}', '{userAccount.Email}')
								SET IDENTITY_INSERT {UserTableName} OFF";

                    await context.Database.ExecuteSqlCommandAsync(query);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToDetail());
            }
        }

    }
}