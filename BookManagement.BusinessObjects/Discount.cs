using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects
{
    public class Discount
    {
        public int DiscountID { get; set; }
        public string discountName { get; set; }
        public double discountValue { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }

}
