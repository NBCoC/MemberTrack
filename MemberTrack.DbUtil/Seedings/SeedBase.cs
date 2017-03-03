using MemberTrack.Common;
using MemberTrack.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MemberTrack.DbUtil.Seedings
{
    abstract class SeedBase
    {
        protected readonly DatabaseContext _databaseContext;
        protected readonly bool _forceReseeding;

        public SeedBase(DatabaseContext databaseContext, bool forceReseeding)
        {
            if (databaseContext == null)
                throw new ArgumentNullException(nameof(databaseContext));

            _databaseContext = databaseContext;
            _forceReseeding = forceReseeding;
        }

        protected abstract void ClearData();
        protected abstract void PopulateData();

        public virtual void Seed()
        {
            if (_forceReseeding)
            {
                ClearData();
            }

            PopulateData();
        }

        protected void ExecuteSql(string relativeFilePath)
        {
            var file = $@"{AppDomain.CurrentDomain.BaseDirectory}\{relativeFilePath}";
            try
            {

                if (!File.Exists(file))
                {
                    throw new InvalidOperationException($"{file} not found.");
                }

                string sql;

                using (var reader = File.OpenText(file))
                {
                    sql = reader.ReadToEnd();
                }

                var commands =
                    Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase).Where(
                    str => !string.IsNullOrEmpty(str));

                foreach (var command in commands)
                {
                    _databaseContext.Database.ExecuteSqlCommand(command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Executing file {file} against the database failed.");
                Console.WriteLine(e.ToDetail());
            }
        }

    }
}
