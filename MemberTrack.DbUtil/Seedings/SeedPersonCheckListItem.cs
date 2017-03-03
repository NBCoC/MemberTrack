using MemberTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.DbUtil.Seedings
{
    class SeedPersonCheckListItem : SeedBase
    {

        public SeedPersonCheckListItem(DatabaseContext databaseContext, bool forceReseeding)
            : base(databaseContext, forceReseeding)
        {
        }

        protected override void ClearData()
        {
            _databaseContext.Database.ExecuteSqlCommand($"TRUNCATE TABLE dbo.PersonCheckListItem");
        }

        protected override void PopulateData()
        {
            ExecuteSql(@"Seedings\SeedPersonCheckListItem.sql");
        }
    }
}
