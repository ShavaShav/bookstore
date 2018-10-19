using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStoreLib
{
    [TestClass]
    public class TestRemoveBook
    {
        const string testOrderID = "1";
        OrderItem testOrderItem = new OrderItem(testOrderID, "Test Book", 1.0, 1);
        OrderItem otherOrderItem = new OrderItem("2", "Not the expected book", 1.0, 1);

        [TestMethod]
        public void RemoveBookSuccess()
        {
            // Add test order to list, assert in list
            BookOrder bookOrder = new BookOrder();
            bookOrder.AddItem(testOrderItem);
            Assert.IsTrue(bookOrder.OrderItemList.Contains(testOrderItem));

            // Remove from list using ID, assert removed from list
            bookOrder.RemoveItem(testOrderID);
            Assert.IsFalse(bookOrder.OrderItemList.Contains(testOrderItem));
            Assert.AreEqual(bookOrder.OrderItemList.Count, 0);
        }

        [TestMethod]
        public void RemoveBookNotInList()
        {
            // Add other order to list, assert in list
            BookOrder bookOrder = new BookOrder();
            bookOrder.AddItem(otherOrderItem);
            Assert.IsTrue(bookOrder.OrderItemList.Contains(otherOrderItem));

            // Attempt remove test order from list using ID, list unchanged
            bookOrder.RemoveItem(testOrderID);
            Assert.IsTrue(bookOrder.OrderItemList.Contains(otherOrderItem));
            Assert.IsFalse(bookOrder.OrderItemList.Contains(testOrderItem));
            Assert.AreEqual(bookOrder.OrderItemList.Count, 1);
        }

        [TestMethod]
        public void RemoveBookEmptyList()
        {
            // Create list, assert empty
            BookOrder bookOrder = new BookOrder();
            Assert.AreEqual(bookOrder.OrderItemList.Count, 0);

            // Attempt remove test order from list using ID, list unchanged
            bookOrder.RemoveItem(testOrderID);
            Assert.IsFalse(bookOrder.OrderItemList.Contains(testOrderItem));
            Assert.AreEqual(bookOrder.OrderItemList.Count, 0);
        }
    }
}
