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
        public string authorName { get; set; }
        public DateTime? authorDOB { get; set; }
        public string? authorImageURL { get; set; }
        public string? authorEmail { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
