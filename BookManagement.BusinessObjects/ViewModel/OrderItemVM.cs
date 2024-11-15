using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects.ViewModel
{
   public class OrderItemVM
    {
        public int BookID { get; set; }
        public int OrderID { get; set; }
        public string Title { get; set; }
        public string BookImages { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string FormattedQuantity => $"x{Quantity}";

        // Property to format price with currency symbol (e.g., "₫66.000")
        public string FormattedPrice => $"${Price:N0}";
    }
}
