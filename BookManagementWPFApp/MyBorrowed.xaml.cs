using BookManagement.BusinessObjects.ViewModel;
using BookManagement.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Util;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for MyBorrowed.xaml
    /// </summary>
    public partial class MyBorrowed : Page
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMyMapper _mapper;
        public MyBorrowed()
        {
            InitializeComponent();
            _bookRepository = new BookRepository();
            _mapper = new MyMapper();
            LoadBorrowedBooks();
        }
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var card = sender as MaterialDesignThemes.Wpf.Card;

            if (card != null)
            {
                int bookID = (int)card.Tag;
                NavigationService?.Navigate(new BookDetailsPage(bookID));
            }
        }
        private void LoadBorrowedBooks()
        {
            var userId = int.Parse(Application.Current.Properties["UserID"].ToString());
                   var books = _bookRepository.GetBorrowedBooksOfUser(userId);
            var bookVMs = new List<BookVM>();
            foreach (var book in books)
            {
                BookVM bookVm = new BookVM();
                _mapper.Map(book, bookVm);
                bookVMs.Add(bookVm);
            }
            ic_books.ItemsSource = new ObservableCollection<BookVM>(bookVMs).ToList<BookVM>();
        }
    }
}
