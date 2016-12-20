using System.Collections.Generic;
using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities
{
    public class VisitCheckListItem : IEntity
    {
        public string Description { get; set; }

        public long Id { get; set; }

        public int Group { get; set; }

        public virtual ICollection<VisitCheckList> CheckList { get; set; } = new List<VisitCheckList>();
    }
}