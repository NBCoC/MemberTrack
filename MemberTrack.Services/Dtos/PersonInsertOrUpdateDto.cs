using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class PersonInsertOrUpdateDto
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public PersonStatusEnum Status { get; set; }

        public GenderEnum Gender { get; set; }

        public AgeGroupEnum? AgeGroup { get; set; }
    }
}