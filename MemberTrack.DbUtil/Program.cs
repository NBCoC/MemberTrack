namespace MemberTrack.DbUtil
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Microsoft.EntityFrameworkCore;

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
                    Console.WriteLine("Forcing the reseeding of the system accounts");
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
            const string userTableName = "dbo.[User]";

            try
            {
                var seeded = await context.Users.AnyAsync(x => SystemAccountHelper.IsSystemAccount(x.Id));

                if (seeded && !forced) return;

                foreach (var userAccount in SystemAccountHelper.SystemAccounts)
                {
                    if (forced)
                    {
                        context.Database.ExecuteSqlCommand(
                            $"DELETE FROM {userTableName} WHERE Email = '{userAccount.Email}'");
                    }

                    var query = $@"SET IDENTITY_INSERT {userTableName} ON
								INSERT INTO {userTableName}
									(Id, DisplayName, Role, Password, Email) 
								VALUES({userAccount.Id}, '{userAccount.DisplayName}', 
                    {(int) userAccount.Role}, '{userAccount.Password}', '{userAccount.Email}')
								SET IDENTITY_INSERT {userTableName} OFF";

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