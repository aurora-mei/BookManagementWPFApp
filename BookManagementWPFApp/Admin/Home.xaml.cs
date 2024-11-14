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
using Microsoft.Win32;
using System.IO;
using BookManagement.BusinessObjects;
using BookManagementWPFApp.Admin.VM;

namespace BookManagementWPFApp.Admin
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private readonly IBookRepository _bookRepository;
        public Home()
        {
            _bookRepository = new BookRepository();
            InitializeComponent();
            ListBook();
        }

        private void ListBook()
        {
            var books = _bookRepository.GetListBooks();
            var bookVMs = books.Select(b => new BookVM
            {
                BookID = b.BookID,
                Title = b.Title,
                Price = b.Price,
                Quantity = b.Quantity,
                PublishDate = b.PublishDate,
                CategoryName = b.Category.CategoryName,
                Description = b.Description,
                authorName = b.Author.authorName,
                Language = b.Language,
                discountValue = b.Discount.discountValue,
                BookImages = b.BookImages
                
            }).ToList();
            dg_Data.ItemsSource = bookVMs;
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dg_Data.SelectedItem == null)
            {
                MessageBox.Show("Please select a book to delete");
                return;
            }
            else
            {
                var book = dg_Data.SelectedItem as BookVM;
                _bookRepository.DeleteBook(book.BookID);
                ListBook();
            }
        }

        private void btn_Upload_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddBookDetail());
        }

        private void dg_Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_Data.SelectedItem == null)
            {
                return;
            }
            else if (dg_Data.SelectedItem != null)
            {
                var selectedBook = dg_Data.SelectedItem as BookVM;
                var bookDetail = new BookDetail(selectedBook);

                NavigationService.Navigate(bookDetail);
            }
        }
    }
}
