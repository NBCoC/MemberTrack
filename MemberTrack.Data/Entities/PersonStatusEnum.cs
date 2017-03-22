using System.ComponentModel;

namespace MemberTrack.Data.Entities
{
    public enum PersonStatusEnum
    {
        [Description("Unknown")] Unknown = 0,
        [Description("Visitor")] Visitor = 1,
        [Description("Member")] Member = 2
    }
}