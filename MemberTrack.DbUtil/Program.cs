using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

                using (var dbContext = new DatabaseContextFactory().Create(arguments.Datasource, arguments.Catalog))
                {
                    Console.WriteLine("Runnig migration script...");
                    ExecuteSql(dbContext).Wait();
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
								VALUES({SystemAccountHelper.UserId}, 'Alberto De Pena', 
                    {(int) UserRoleEnum.SystemAdmin}, '{SystemAccountHelper.Password}', '{SystemAccountHelper.Email}')
								SET IDENTITY_INSERT dbo.[User] OFF";

                await context.Database.ExecuteSqlCommandAsync(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToDetail());
            }
        }

        private static async Task ExecuteSql(DbContext context)
        {
            try
            {
                var file = $@"{AppDomain.CurrentDomain.BaseDirectory}\MemberTrack.sql";

                if (!File.Exists(file))
                {
                    throw new InvalidOperationException($"{file} not found.");
                }

                string sql;

                using (var reader = File.OpenText(file))
                {
                    sql = await reader.ReadToEndAsync();
                }

                var commands =
                    Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase).Where(
                        str => !string.IsNullOrEmpty(str));

                foreach (var command in commands)
                {
                    await context.Database.ExecuteSqlCommandAsync(command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToDetail());
            }
        }
    }
}