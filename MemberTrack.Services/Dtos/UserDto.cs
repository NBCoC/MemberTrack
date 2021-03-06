using MemberTrack.Common;
using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public UserRoleEnum Role { get; set; }

        public string RoleName => Role.ToDescription();

        public bool IsSystemAdmin => Role == UserRoleEnum.SystemAdmin;

        public bool IsAdmin => IsSystemAdmin || Role == UserRoleEnum.Admin;

        public bool IsEditor => IsAdmin || Role == UserRoleEnum.Editor;
    }
}