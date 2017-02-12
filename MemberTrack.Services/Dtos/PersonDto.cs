using System.Collections.Generic;
using MemberTrack.Common;

namespace MemberTrack.Services.Dtos
{
    public class PersonDto : PersonInsertOrUpdateDto
    {
        public string StatusName => Status.ToDescription();

        public string GenderName => Gender.ToDescription();

        public string AgeGroupName => AgeGroup?.ToDescription();

        public AddressDto Address { get; set; }

        public ChildrenInfoDto ChildrenInfo { get; set; }

        public IEnumerable<PersonCheckListItemDto> CheckListItems { get; set; } = new List<PersonCheckListItemDto>();

        public long Id { get; set; }
    }
}