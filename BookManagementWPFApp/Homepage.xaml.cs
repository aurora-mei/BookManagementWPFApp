using BookManagement.BusinessObjects;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMyMapper _mapper;
        public HomePage()
        {
            InitializeComponent();
            _bookRepository = new BookRepository();
            _mapper = new MyMapper();
            _orderRepository = new OrderRepository();
            _orderItemRepository = new OrderItemRepository();
            LoadBooks();
        }
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Sử dụng đúng kiểu Card từ MaterialDesignThemes.Wpf
            var card = sender as MaterialDesignThemes.Wpf.Card;

            if (card != null)
            {
                int bookID = (int)card.Tag;
                NavigationService?.Navigate(new BookDetailsPage(bookID));
            }
        }

        private void LoadBooks()
        {
            var books = _bookRepository.GetListBooks();
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
