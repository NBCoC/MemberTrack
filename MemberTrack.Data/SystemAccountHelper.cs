namespace MemberTrack.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class SystemAccountHelper
    {
        // P@55word
        public const string DefaultPassword = "ELct6518+hf+4AuYvous+362kW/J2JRCbqyio2er0No=";

        public static readonly List<User> SystemAccounts = new List<User>
        {
            new User
            {
                Id = -1989,
                DisplayName = "Alberto De Pena",
                Email = "adepena@nbchurchfamily.org",
                Password = "XZDRUEcVwG++2GKU0zZKl2X0tSyj+FmHlAHBWPJ5sEk=",
                Role = UserRoleEnum.SystemAdmin
            },
            new User
            {
                Id = -1971,
                DisplayName = "David Maltby",
                Email = "dmaltby@nbchurchfamily.org",
                Password = "tjWdEz3003Oxgcfjt6WG6XFQUX6HhHg/I0o+MkCr4wM=",
                Role = UserRoleEnum.SystemAdmin
            }
        };

        public static bool IsSystemAccount(long id)
            => SystemAccounts.Any(user => user.Id == id && user.Role == UserRoleEnum.SystemAdmin);

        public static bool IsSystemAccount(string email)
            => SystemAccounts.Any(user => user.Email == email && user.Role == UserRoleEnum.SystemAdmin);
    }
}