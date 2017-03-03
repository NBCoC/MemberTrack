namespace MemberTrack.Services.Dtos
{
    using System;
    using Data.Entities;

    public class RecentPersonDto
    {
        private DateTimeOffset? _lastModifiedDate;

        public long Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? LastModifiedDate
        {
            get { return _lastModifiedDate ?? Date; }
            set { _lastModifiedDate = value; }
        }

        public bool RequiresAttention { get; set; }

        public PersonStatusEnum Status { get; set; }

        public DateTimeOffset? Date { get; set; }
    }
}