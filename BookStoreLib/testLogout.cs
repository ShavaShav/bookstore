using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStoreLib
{
    [TestClass]
    public class TestLogout
    {
        // Test Case #1
        [TestMethod]
        public void LogoutSuccess()
        {
            // login
            Account.Login("shaverz", "zs1234");
            Assert.IsTrue(Account.IsLoggedIn);

            // logout
            Account.Logout();
            Assert.IsFalse(Account.IsLoggedIn);
            Assert.AreEqual(Account.currentUser, null);
        }
    }
}
