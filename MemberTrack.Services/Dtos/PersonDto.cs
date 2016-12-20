using System.Collections.Generic;
using MemberTrack.Common;

namespace MemberTrack.Services.Dtos
{
    public class PersonDto : PersonInsertOrUpdateDto
    {
        public string StatusName => Status.ToDescription();

        public string GenderName => Gender.ToDescription();

        public string AgeGroupName => AgeGroup.ToDescription();

        public AddressDto Address { get; set; }

        public ChildrenInfoDto ChildrenInfo { get; set; }

        public IEnumerable<VisitDto> Visits { get; set; } = new List<VisitDto>();

        public long Id { get; set; }
    }
}