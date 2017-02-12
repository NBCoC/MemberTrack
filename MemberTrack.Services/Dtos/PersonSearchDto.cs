using MemberTrack.Common;
using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class PersonSearchDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public PersonStatusEnum Status { get; set; }

        public string StatusName => Status.ToDescription();

        public AgeGroupEnum? AgeGroup { get; set; }

        public string AgeGroupName => AgeGroup?.ToDescription();
    }
}