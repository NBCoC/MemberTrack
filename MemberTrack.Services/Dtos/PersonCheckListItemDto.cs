using System;

namespace MemberTrack.Services.Dtos
{
    public class PersonCheckListItemDto : PersonCheckListItemLookupDto
    {
        public string Note { get; set; }

        public DateTimeOffset? Date { get; set; }
    }
}