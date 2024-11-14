using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects.ViewModel
{
    public class BookVM
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BookImages { get; set; }
        public string BookAvatar{ get; set; }
        public double Price { get; set; }
        public int Pages { get; set; }
        public int Quantity { get; set; }
        public DateTime PublishDate { get; set; }
        public string Language { get; set; }
        public double? DiscountValue { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string? AuthorImageURL { get; set; }
    }
}
