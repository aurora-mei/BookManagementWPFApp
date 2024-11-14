using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for ExBorrrow.xaml
    /// </summary>
    public partial class ExBorrrow : Window
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private Book _selectedBook;
        private User _currentUser;
        public ExBorrrow(Book selectedBook, User currentUser)
        {
            _bookRepository = new BookRepository();
            _selectedBook = selectedBook;
            _currentUser = currentUser;
            _loanRepository = new LoanRepository();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook == null || _currentUser == null) return;
            var currentBookLoans = _loanRepository.GetLoan(l => l.BookID == _selectedBook.BookID);
            switch (currentBookLoans.Count)
            {
                case < 5:
                {
                    var newLoan = new Loan
                    {
                        BookID = _selectedBook.BookID,
                        UserID = _currentUser.UserID,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(5),
                        Status = 0,//loan
                        FineAmount = _selectedBook.Price * 10 / 100
                    };
                    _loanRepository.AddLoan(newLoan);
                    break;
                }
                case >= 5 when currentBookLoans[0].DueDate < DateTime.Now:
                    currentBookLoans[0].UserID = _currentUser.UserID;
                    currentBookLoans[0].BorrowDate = DateTime.Now;
                    currentBookLoans[0].DueDate = DateTime.Now.AddDays(5);
                    break;
                case >= 5 when currentBookLoans[0].DueDate >= DateTime.Now:
                    var waitLoan = new Loan
                    {
                        BookID = _selectedBook.BookID,
                        UserID = _currentUser.UserID,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(5),
                        Status = 1,//wait
                        FineAmount = _selectedBook.Price * 10 / 100
                    };
                    _loanRepository.AddLoan(waitLoan);
                    break;
            }
        }
    }
}
