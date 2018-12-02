using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLib
{
    public class PurchaseObj
    {
        public string OrderID { get; set; }
        public string Date { get; set; }
        public string ISBN { get; set; }
        public string Quantity { get; set; }
        public string Status { get; set; }
        public PurchaseObj(string OrderID, string Date, string ISBN, string Quantity, string Status)
        {
            this.OrderID = OrderID;
            this.Date = Date;
            this.ISBN = ISBN;
            this.Quantity = Quantity;
            this.Status = Status;
        }
    }
}
