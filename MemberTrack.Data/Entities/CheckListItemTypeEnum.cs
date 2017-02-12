using System.ComponentModel;

namespace MemberTrack.Data.Entities
{
    public enum CheckListItemTypeEnum
    {
        [Description("Unknown")] Unknown = 1,
        [Description("1st Visit")] FirstVisit,
        [Description("2nd Visit")] SecondVisit,
        [Description("3rd Visit")] ThirdVisit,
        [Description("4th Visit")] FourthVisit,
        [Description("Membership Request Date")] MembershipRequestDate,
        [Description("Membership Announcement Date")] MembershipAnnouncementDate,
        [Description("Life Group")] LifeGroup,
        [Description("Ministry")] Ministry
    }
}