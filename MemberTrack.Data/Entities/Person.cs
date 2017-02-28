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

        public DateTimeOffset? FirstVisitDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public GenderEnum Gender { get; set; }

        public AgeGroupEnum AgeGroup { get; set; }

        public bool HasInfantKids { get; set; }

        public bool HasToddlerKids { get; set; }

        public bool HasElementaryKids { get; set; }

        public bool HasJuniorHighKids { get; set; }

        public bool HasHighSchoolKids { get; set; }

        public virtual ICollection<PersonCheckList> CheckLists { get; set; } = new List<PersonCheckList>();

        public long Id { get; set; }
    }
}