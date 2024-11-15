using BookManagement.DataAccess.Repositories;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookManagement.BusinessObjects;
using Util;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for BookDetailsPage.xaml
    /// </summary>
    public partial class BookDetailsPage : Page
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoanRepository _loanRepository;
        public int BookID { get; set; }
        public BookDetailsPage(int BookID)
        {
            InitializeComponent();
            this.BookID = BookID;
            _bookRepository = new BookRepository();
            _userRepository = new UserRepository();
            _loanRepository = new LoanRepository();
            LoadBookDetails();
        }
        private void LoadBookDetails()
        {
            var book = _bookRepository.GetBookById(BookID);
            img_book.ImageSource = new BitmapImage(new Uri(book.BookImages.Split(",")[0]));
           if(string.IsNullOrEmpty(book.Author.AuthorImageURL))
            {
                img_author.ImageSource = new BitmapImage(new Uri(book.BookImages));
            }
            tb_author.Text = book.Author.AuthorName;
            tb_bookName.Text = book.Title;
            tb_authorName.Text = book.Author.AuthorName;
            tb_price.Text = book.Price.ToString() + "  ";
            tb_bookDescription.Text = book.Description;

            tb_bookLanguage.Text = book.Language;
            tb_bookPages.Text = book.Pages.ToString();
            tb_bookPublishDate.Text = book.PublishDate.ToString("dd/MM/yyyy");
            tb_bookQuantity.Text = book.Quantity.ToString();
            
        }

        private void Btn_borrow_OnClick(object sender, RoutedEventArgs e)
        {
            var book = _bookRepository.GetBookById(BookID);
            var currentUserId = int.Parse(Application.Current.Properties["UserID"].ToString());
            var currentUser = _userRepository.GetUser(u => u.UserID == currentUserId);
            if (book == null || currentUser == null) return;
            var currentBookLoans = _loanRepository.GetLoan(l => l.BookID == book.BookID);
            switch (currentBookLoans.Count)
            {
                case < 5:
                {
                    var newLoan = new Loan
                    {
                        BookID = book.BookID,
                        UserID = currentUserId,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(5),
                        Status = LoanStatusConstant.Borrowed,
                        FineAmount = book.Price * 25 / 100
                    };
                    _loanRepository.AddLoan(newLoan);
                    break;
                }
                case >= 5 when currentBookLoans[0].DueDate < DateTime.Now:
                    currentBookLoans[0].UserID = currentUserId;
                    currentBookLoans[0].BorrowDate = DateTime.Now;
                    currentBookLoans[0].DueDate = DateTime.Now.AddDays(5);
                    break;
                case >= 5 when currentBookLoans[0].DueDate >= DateTime.Now:
                    var waitLoan = new Loan
                    {
                        BookID = book.BookID,
                        UserID = currentUserId,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(5),
                        Status = LoanStatusConstant.Waiting,
                        FineAmount = book.Price * 25 / 100
                    };
                    _loanRepository.AddLoan(waitLoan);
                    break;
            }
        }
    }
}
