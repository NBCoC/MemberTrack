using System.ComponentModel;

namespace MemberTrack.Data.Entities
{
    public enum AgeGroupEnum
    {
        [Description("Unknown")] Unknown = 0,
        [Description("18 - 29")] Group1 = 1,
        [Description("29 - 39")] Group2 = 2,
        [Description("40 - 50")] Group3 = 3,
        [Description("50 - 59")] Group4 = 4,
        [Description("60+")] Group5 = 5
    }
}