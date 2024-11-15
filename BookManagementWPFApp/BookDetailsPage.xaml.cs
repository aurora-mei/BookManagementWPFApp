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

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for BookDetailsPage.xaml
    /// </summary>
    public partial class BookDetailsPage : Page
    {
        private readonly IBookRepository _bookRepository;
        public int BookID { get; set; }
        public BookDetailsPage(int BookID)
        {
            InitializeComponent();
            this.BookID = BookID;
            _bookRepository = new BookRepository();
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
    }
}
