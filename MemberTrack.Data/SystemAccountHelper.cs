using MemberTrack.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MemberTrack.Data
{
    public class SystemAccountHelper
    {
        //TODO:  Hash Maltby's password
        public static readonly List<User> SystemAccounts = new List<User>()
                {
                    new User()
                    {
                        Id = -1989,
                        DisplayName = "Alberto De Pena",
                        Email = "adepena@nbchurchfamily.org",
                        // membertrack2017
                        Password = "6rLwqD6BaHVwgeMgTavh5OO4zNnbk9buNFi10T+SdNo=",
                        //Password = "XZDRUEcVwG++2GKU0zZKl2X0tSyj+FmHlAHBWPJ5sEk=",
                        Role = UserRoleEnum.SystemAdmin,
                    },
                    new User()
                    {
                        Id = -1971,
                        DisplayName = "David Maltby",
                        Email = "dmaltby@nbchurchfamily.org",
                        Password = "tjWdEz3003Oxgcfjt6WG6XFQUX6HhHg/I0o+MkCr4wM=",
                        Role = UserRoleEnum.SystemAdmin,
                    },
                };

        // P@55word
        public const string DefaultPassword = "ELct6518+hf+4AuYvous+362kW/J2JRCbqyio2er0No=";

        public static bool IsSystemAccount(long id)
        {
            return SystemAccounts.Any(user => user.Id == id && user.Role == UserRoleEnum.SystemAdmin);
        }

        public static bool IsSystemAccount(string email)
        {
            return SystemAccounts.Any(user => user.Email == email && user.Role == UserRoleEnum.SystemAdmin);
        }
    }
}