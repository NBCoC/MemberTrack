namespace MemberTrack.DbUtil
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Data.Entities;
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

                using (var dbContext = new DatabaseContextFactory().Create(arguments.Datasource, arguments.Catalog))
                {
                    Console.WriteLine("Creating system account...");
                    SeedSystemAccount(dbContext).Wait();
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

        private static async Task SeedSystemAccount(DatabaseContext context)
        {
            try
            {
                var seeded = await context.Users.AnyAsync(x => x.Id == SystemAccountHelper.UserId);

                if (seeded) return;

                var query = $@"SET IDENTITY_INSERT dbo.[User] ON
								INSERT INTO dbo.[User]
									(Id, DisplayName, Role, Password, Email) 
								VALUES({SystemAccountHelper.UserId}, 'MemberTrack', 
                    {(int) UserRoleEnum.SystemAdmin}, '{SystemAccountHelper.Password}', '{SystemAccountHelper.Email}')
								SET IDENTITY_INSERT dbo.[User] OFF";

                await context.Database.ExecuteSqlCommandAsync(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToDetail());
            }
        }
    }
}