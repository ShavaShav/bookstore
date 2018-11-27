/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BookStoreLib
{
    public class BookOrder
    {
        ObservableCollection<OrderItem> orderItemList = new
            ObservableCollection<OrderItem>();
        public ObservableCollection<OrderItem> OrderItemList
        {
            get { return orderItemList; }
        }

        public double getCDNTotal()
        {
            return GetOrderTotal() * 1.32;
        }

        public void AddItem(OrderItem orderItem)
        {
            OrderItem changedItem = null, oldItem = null;
            foreach (var item in orderItemList)
            {
                if (item.BookID == orderItem.BookID)
                {
                    oldItem = item;
                    item.Quantity += orderItem.Quantity;
                    item.SubTotal += orderItem.SubTotal;
                    changedItem = item;
                }
            }
            if (oldItem != null)
            {
                orderItemList.Remove(oldItem);
                orderItemList.Add(changedItem);
            }
            else
                orderItemList.Add(orderItem);
        }
        public void RemoveItem(string productID)
        {
            foreach (var item in orderItemList)
            {
                if (item.BookID == productID)
                {
                    orderItemList.Remove(item);
                    return;
                }
            }
        }
        public double GetOrderTotal()
        {
            double total = 0;
            foreach (var item in orderItemList)
            {
                total += item.SubTotal;
            }
            return total;
        }
        public int PlaceOrder(int userID)
        {
            string xmlOrder;
            xmlOrder = "<Order UserID='" + userID.ToString() + "'>";
            foreach (var item in orderItemList)
            {
                xmlOrder += item.ToString();
            }
            xmlOrder += "</Order>";
            DALOrder dbOrder = new DALOrder();
            return dbOrder.Proceed2Order(xmlOrder);
        }
    }
}
