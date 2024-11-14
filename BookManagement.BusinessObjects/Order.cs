using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookManagement.BusinessObjects
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string ShippingMethod { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public virtual User User { get; set; }
        public virtual IEnumerable<OrderItem> OrderItems { get; set; }

    }
}
