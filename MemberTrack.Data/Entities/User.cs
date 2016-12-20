using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities
{
    public class User : IEntity
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public UserRoleEnum Role { get; set; }
    }
}