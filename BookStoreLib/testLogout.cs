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
            User testUser = new User();
            testUser.Login("shaverz", "zs1234");
            Assert.IsTrue(testUser.IsLoggedIn);

            int expectedId = -1;

            // logout
            testUser.logout();
            Assert.IsFalse(testUser.IsLoggedIn);
            Assert.AreEqual(testUser.Id, expectedId);
        }
    }
}
