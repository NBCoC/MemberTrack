using System.ComponentModel;

namespace MemberTrack.Data.Entities
{
    public enum AgeGroupEnum
    {
        [Description("Unknown")] Unknown = 1,
        [Description("18 - 29")] Group1,
        [Description("29 - 39")] Group2,
        [Description("40 - 50")] Group3,
        [Description("50 - 59")] Group4,
        [Description("60+")] Group5
    }
}