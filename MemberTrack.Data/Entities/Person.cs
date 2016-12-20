using System;
using System.Collections.Generic;
using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities
{
    public class Person : IEntity
    {
        public virtual Address Address { get; set; }

        public string ContactNumber { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public PersonStatusEnum Status { get; set; }

        public DateTimeOffset? MembershipDate { get; set; }

        public DateTimeOffset? BaptismDate { get; set; }

        public GenderEnum Gender { get; set; }

        public AgeGroupEnum AgeGroup { get; set; }

        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

        public virtual ICollection<ChildrenInfo> ChildrenInfos { get; set; } = new List<ChildrenInfo>();

        public long Id { get; set; }
    }
}