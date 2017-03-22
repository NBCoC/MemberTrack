using MemberTrack.Data;
using MemberTrack.Data.Seedings;
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
            _databaseContext.Database.ExecuteSqlCommand($"DELETE FROM dbo.PersonCheckListItem");
        }

        protected override void PopulateData()
        {
            ExecuteSql(@"Seedings\SeedPersonCheckListItem.sql");
        }
    }
}
