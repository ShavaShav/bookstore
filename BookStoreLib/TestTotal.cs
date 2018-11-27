using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStoreLib
{
    [TestClass]
    public class TestTotal
    {
        const string testOrderID = "1";
        OrderItem testOrderItem = new OrderItem(testOrderID, "Test Book", 100, 2);


        [TestMethod]
        public void fromUSDtoCAD()
        {
            // Add test order to list, assert in list
            BookOrder bookOrder = new BookOrder();
            bookOrder.AddItem(testOrderItem);
            Assert.AreEqual(bookOrder.getCDNTotal(), 200*1.32);
        }

        [TestMethod]
        public void fromCADtoUSD()
        {
            // Add test order to list, assert in list
            BookOrder bookOrder = new BookOrder();
            bookOrder.AddItem(testOrderItem);
            Assert.AreEqual(bookOrder.GetOrderTotal(), 200);
        }

    }
}
