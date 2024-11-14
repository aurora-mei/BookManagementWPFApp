using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Pages { get; set; }
        public int Quantity { get; set; }
        public DateTime PublishDate { get; set; }
        public int CategoryID { get; set; }
        public string Description { get; set; }
        public int AuthorID { get; set; }
        public string Language { get; set; }
        public int? DiscountID { get; set; }
        public string BookPDFLink { get; set; }
        public string BookImages { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Author Author { get; set; }
        public virtual Discount? Discount { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Loan>? Loans { get; set; }

    }
}
