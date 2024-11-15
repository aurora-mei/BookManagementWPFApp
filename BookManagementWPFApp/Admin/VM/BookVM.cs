using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementWPFApp.Admin.VM
{
    public class BookVM
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PublishDate { get; set; }
        // *Category
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        // --------------------------------------
        public string Description { get; set; }
        public string? BookPDFLink { get; set; }
        public string? BookImages { get; set; }
        public string Language { get; set; }

        // --------------------------------------

        // *Author
        public int AuthorID { get; set; }
        public string authorName { get; set; }
        public string? authorEmail { get; set; }

        // *Discount
        public int DiscountID { get; set; }
        public double discountValue { get; set; }
    }
}
