namespace MemberTrack.Data.Entities
{
    public class ChildrenInfo
    {
        public long PersonId { get; set; }

        public virtual Person Person { get; set; }

        public AgeGroupEnum AgeGroup { get; set; }
    }
}