using System.Collections.Generic;
using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class ChildrenInfoDto
    {
        public IEnumerable<AgeGroupEnum> AgeGroups { get; set; } = new List<AgeGroupEnum>();
    }
}