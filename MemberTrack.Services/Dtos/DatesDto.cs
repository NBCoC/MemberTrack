namespace MemberTrack.Services.Dtos
{
    using System;

    public class DatesDto
    {
        public DateTimeOffset? MembershipDate { get; set; }

        public DateTimeOffset? BaptismDate { get; set; }

        public DateTimeOffset? FirstVisitDate { get; set; }
    }
}