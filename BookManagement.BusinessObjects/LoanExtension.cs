using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.BusinessObjects
{
  
    public class LoanExtension
    {
        public int LoanItemID { get; set; }
        public int LoanID { get; set; }
        public DateTime ExtensionDate { get; set; }
        public DateTime ExtendedDueDate { get; set; }
        public virtual Loan Loan { get; set; }
    }
}
