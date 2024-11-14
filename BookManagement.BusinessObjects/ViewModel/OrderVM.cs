using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects.ViewModel
{
    public class OrderVM
    {
        public int OrderID { get; set; }
        public string OrderTitle { get; set; } // e.g., "Order 1"
        public double TotalPrice { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }
    }
}
