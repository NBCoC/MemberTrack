namespace MemberTrack.Data.Entities
{
    public class VisitCheckList
    {
        public long VisitorId { get; set; }

        public virtual Visit Visit { get; set; }

        public long VisitCheckListItemId { get; set; }

        public virtual VisitCheckListItem VisitCheckListItem { get; set; }
    }
}