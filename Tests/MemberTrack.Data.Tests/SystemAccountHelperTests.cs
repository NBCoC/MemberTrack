using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Data.Tests
{
    [TestClass]
    public class SystemAccountHelperTests
    {
        [TestMethod]
        public void AllSystemAccountsInSystemAdminRole()
        {
            var systemAccounts = SystemAccountHelper.SystemAccounts;
            Assert.IsTrue(systemAccounts.All(accounts => accounts.Role == Entities.UserRoleEnum.SystemAdmin));
        }

    }
}
