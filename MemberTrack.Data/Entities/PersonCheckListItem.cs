using System.Collections.Generic;
using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities
{
    public class PersonCheckListItem : IEntity
    {
        public string Description { get; set; }

        public CheckListItemTypeEnum CheckListItemType { get; set; }

        public virtual ICollection<PersonCheckList> CheckLists { get; set; } = new List<PersonCheckList>();

        public int SortOrder { get; set; }

        public long Id { get; set; }
    }
}