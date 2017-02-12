using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class UserInsertDto
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public UserRoleEnum Role { get; set; }
    }
}