using System;
using System.Collections.Generic;
using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities
{
    public class Person : IEntity
    {
        public string Email { get; set; }

        public string FullName { get; set; }
        
        public PersonStatusEnum Status { get; set; }

        public DateTimeOffset? MembershipDate { get; set; }
        
        public DateTimeOffset? FirstVisitDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        
        public AgeGroupEnum AgeGroup { get; set; }
        
        public virtual ICollection<PersonCheckList> CheckLists { get; set; } = new List<PersonCheckList>();

        public long Id { get; set; }
    }
}