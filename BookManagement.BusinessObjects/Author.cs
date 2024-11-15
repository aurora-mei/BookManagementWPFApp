using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects
{
   
    public class Author
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public DateTime? AuthorDOB { get; set; }
        public string? AuthorImageURL { get; set; }
        public string? AuthorEmail { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
