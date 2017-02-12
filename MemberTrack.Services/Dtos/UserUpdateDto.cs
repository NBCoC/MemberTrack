using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class UserUpdateDto
    {
        public string DisplayName { get; set; }

        public UserRoleEnum Role { get; set; }
    }
}