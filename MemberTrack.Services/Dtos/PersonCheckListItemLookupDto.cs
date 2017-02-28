using MemberTrack.Common;
using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class PersonCheckListItemLookupDto
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public CheckListItemTypeEnum Type { get; set; }

        public string TypeName => Type.ToDescription();

        public int SortOrder { get; set; }
    }
}