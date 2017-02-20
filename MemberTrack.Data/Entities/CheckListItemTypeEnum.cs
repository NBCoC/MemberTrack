using System.ComponentModel;

namespace MemberTrack.Data.Entities
{
    public enum CheckListItemTypeEnum
    {
        [Description("Unknown")] Unknown = 0,
        [Description("1st Visit")] FirstVisit = 1,
        [Description("2nd Visit")] SecondVisit = 2,
        [Description("3rd Visit")] ThirdVisit = 3,
        [Description("4th Visit")] FourthVisit = 4,
        [Description("Membership Request Date")] MembershipRequestDate = 5,
        [Description("Membership Announcement Date")] MembershipAnnouncementDate = 6,
        [Description("Life Group")] LifeGroup = 7,
        [Description("Ministry")] Ministry = 8
    }
}