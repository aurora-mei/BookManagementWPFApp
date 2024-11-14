using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookManagement.BusinessObjects
{
    public class OrderItem
    {
        public int BookID{ get; set; }
        public int OrderID{ get; set; }
        public int Quantity{ get; set; }
        public double Price { get; set; }
        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }

    }

}
