namespace MemberTrack.Services.Dtos
{
    public class PersonReportItemDto
    {
        public int MemberCount { get; set; }

        public int VisitorCount { get; set; }

        public int Month { get; set; }

        public string MonthName { get; set; }
    }
}