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
    }
}