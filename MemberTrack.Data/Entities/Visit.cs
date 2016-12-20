using System;
using System.Collections.Generic;

namespace MemberTrack.Data.Entities
{
    public class Visit
    {
        public long VisitorId { get; set; }

        public virtual Person Visitor { get; set; }
        
        public DateTimeOffset Date { get; set; }

        public string Note { get; set; }

        public virtual ICollection<VisitCheckList> CheckList { get; set; } = new List<VisitCheckList>();
    }
}