using System.Collections.Generic;
using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities
{
    public class Document : IEntity
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }

        public string Description { get; set; }

        public virtual DocumentData DocumentData { get; set; }

        public virtual ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    }
}