using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects
{
    public class Loan
    {
        public int LoanID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public int Status { get; set; }// 0 = borrowed, 1 = waiting
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }//5 user mượn cùng lúc, 1 lần mượn là 5 ngày
        public DateTime? ReturnDate { get; set; }
        public double FineAmount { get; set; }
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<LoanExtension> LoanExtensions { get; set; }

    }
}
