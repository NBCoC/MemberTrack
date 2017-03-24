using MemberTrack.Common;
using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class PersonSearchDto
    {
        public long Id { get; set; }

        public string FullName { get; set; }
        
        public PersonStatusEnum Status { get; set; }

        public string StatusName => Status.ToDescription();

        public AgeGroupEnum AgeGroup { get; set; }

        public string AgeGroupName => AgeGroup.ToDescription();
    }
}