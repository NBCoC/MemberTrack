using System;
using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class PersonInsertOrUpdateDto
    {
        public string FullName { get; set; }

        public string ContactNumber { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public PersonStatusEnum Status { get; set; }

        public AgeGroupEnum AgeGroup { get; set; }

        public DateTimeOffset? MembershipDate { get; set; }

        public DateTimeOffset? FirstVisitDate { get; set; }
    }
}