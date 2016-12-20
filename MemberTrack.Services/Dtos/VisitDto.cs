using System;
using System.Collections.Generic;

namespace MemberTrack.Services.Dtos
{
    public class VisitDto
    {
        public string Note { get; set; }

        public DateTimeOffset Date { get; set; }

        public IEnumerable<VisitCheckListItemDto> CheckListItems { get; set; } = new List<VisitCheckListItemDto>();
    }
}