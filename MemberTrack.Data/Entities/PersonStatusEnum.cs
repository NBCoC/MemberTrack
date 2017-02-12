using System.ComponentModel;

namespace MemberTrack.Data.Entities
{
    public enum PersonStatusEnum
    {
        [Description("Visitor")] Visitor = 1,
        [Description("Member")] Member = 2
    }
}