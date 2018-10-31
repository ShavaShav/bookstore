using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStoreLib
{
    class testLogout
    {
        // Test Case #1
        [TestMethod]
        public void LogoutSuccess()
        {
            bool expectedLogoutVal = true;
            User testUser = new User();
            testUser.logout();
            bool actualVal = testUser.IsLoggedIn;
            // Assert on results
            Assert.AreEqual(actualVal, expectedLogoutVal);
        }
    }
}
