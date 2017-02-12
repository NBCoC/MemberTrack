using System;

namespace MemberTrack.Data.Entities
{
    public class PersonCheckList
    {
        public long PersonId { get; set; }

        public virtual Person Person { get; set; }

        public long PersonCheckListItemId { get; set; }

        public virtual PersonCheckListItem PersonCheckListItem { get; set; }

        public string Note { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}