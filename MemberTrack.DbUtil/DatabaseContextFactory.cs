using System;
using MemberTrack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace MemberTrack.DbUtil
{
    public class DatabaseContextFactory : IDbContextFactory<DatabaseContext>
    {
        public DatabaseContext Create(DbContextFactoryOptions options)
        {
            var configBuilder =
                new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json",
                    false, true).Build();

            var connectionString = configBuilder["ConnectionString"];

            return Create(connectionString);
        }

        public DatabaseContext Create(string datasource, string catalog)
        {
            var connectionString =
                $"Data Source={datasource};Initial Catalog={catalog};Trusted_Connection=true;MultipleActiveResultSets=true";

            return Create(connectionString);
        }

        private static DatabaseContext Create(string connectionString)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            var assemblyName = typeof(DatabaseContextFactory).Assembly.GetName().Name;

            dbContextBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName));

            return new DatabaseContext(dbContextBuilder.Options);
        }
    }
}