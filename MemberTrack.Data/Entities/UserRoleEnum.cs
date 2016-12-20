using System.ComponentModel;

namespace MemberTrack.Data.Entities
{
    public enum UserRoleEnum
    {
        [Description("Viewer")] Viewer = 1,
        [Description("Editor")] Editor = 2,
        [Description("Administrator")] Admin = 3,
        [Description("System Administrator")] SystemAdmin = 4
    }
}