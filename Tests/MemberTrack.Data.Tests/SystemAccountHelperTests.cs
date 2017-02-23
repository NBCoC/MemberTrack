using MemberTrack.Common;
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

        [TestMethod]
        public void IsSystemAccount_ID_Success()
        {
            var systemAccounts = SystemAccountHelper.SystemAccounts;

            foreach (var systemAccount in systemAccounts)
            {
                var isSystemAccount = SystemAccountHelper.IsSystemAccount(systemAccount.Id);
                Assert.IsTrue(isSystemAccount);
            }
        }

        [TestMethod]
        public void IsSystemAccount_Email_Success()
        {
            var systemAccounts = SystemAccountHelper.SystemAccounts;

            foreach (var systemAccount in systemAccounts)
            {
                var isSystemAccount = SystemAccountHelper.IsSystemAccount(systemAccount.Email);
                Assert.IsTrue(isSystemAccount);
            }
        }

        [TestMethod]
        public void IsSystemAccount_ID_Failure()
        {
            //Simply test that a few IDs that would be IDENTITY values in the database's User.Id column
            //are NOT system accounts.
            for (int counter = 1; counter < 10; counter++)
            {
                Assert.IsFalse(SystemAccountHelper.IsSystemAccount(counter));
            }
        }

        [TestMethod]
        public void IsSystemAccount_Email_Failure()
        {
            Assert.IsFalse(SystemAccountHelper.IsSystemAccount("joe.blow@nowhere.com"));
            Assert.IsFalse(SystemAccountHelper.IsSystemAccount("remember.the.alamo@texas.independence.com"));
            Assert.IsFalse(SystemAccountHelper.IsSystemAccount("warsteiner.dunkel@wurstfest.com"));
            Assert.IsFalse(SystemAccountHelper.IsSystemAccount("Jesus.Wept@John1135.com"));
        }

        [TestMethod]
        public void DefaultPassword_Verfiy()
        {
            var hashProvider = new HashProvider();
            var hash = hashProvider.Hash("P@55word");
            Assert.AreEqual(SystemAccountHelper.DefaultPassword, hash);
        }
    }
}
