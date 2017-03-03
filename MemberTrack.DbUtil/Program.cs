namespace MemberTrack.DbUtil
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Seedings;

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
                    Console.WriteLine("Seeding the database with data...");
                    var seedManager = new SeedManager(dbContext, arguments.ForceReseeding);
                    seedManager.Seed();
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
    }
}