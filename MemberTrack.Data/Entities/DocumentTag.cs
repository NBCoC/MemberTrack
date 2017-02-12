using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities
{
    public class DocumentTag : IEntity
    {
        public long Id { get; set; }
        
        public virtual Document Document { get; set; }

        public long DocumentId { get; set; }

        public string Value { get; set; }
    }
}