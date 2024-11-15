using BookManagement.BusinessObjects;
using BookManagement.BusinessObjects.ViewModel;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Admin;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Util;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for ManageBookPage.xaml
    /// </summary>
    public partial class ManageBookPage : Page
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IBookRepository _bookRepository;
        private readonly IMyMapper _mapper;
        public ManageBookPage()
        {
            InitializeComponent();
            _categoryRepo = new CategoryRepository();
            _bookRepository = new BookRepository();
            _mapper = new MyMapper();
            LoadCategories();
        }
        private void LoadCategories()
        {
            var cates = _categoryRepo.GetListCategories();
            lb_bookCategories.ItemsSource = new ObservableCollection<Category>(cates).ToList<Category>();
            lb_bookCategories.DisplayMemberPath = "CategoryName";
            if (lb_bookCategories.SelectedItem == null)
            {
                lb_bookCategories.SelectedItem = cates.FirstOrDefault();
            }
        }

        private void lb_bookCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_bookCategories.SelectedItem is Category cate)
            {
                
                var bookVMs = new List<BookVM>();
                var cateObj = _categoryRepo.GetCategoryById(cate.CategoryID);
                foreach (var book in cateObj.Books)
                {
                    var bookVm = new BookVM();
                    _mapper.Map(book, bookVm);
                    bookVMs.Add(bookVm);
                }
                dg_books.ItemsSource = new ObservableCollection<BookVM>(bookVMs).ToList<BookVM>();
            }


        }

        private void icon_deleteRoom_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is PackIcon icon)
            {
                if (icon.DataContext is BookVM book)
                {
                    var bookObj = _bookRepository.GetBookById(book.BookID);
                    _bookRepository.DeleteBook(bookObj);
                    LoadCategories();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete");
                return;
            }
        }

        private void dg_books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_books.SelectedItem == null)
            {
                return;
            }
            else if (dg_books.SelectedItem != null)
            {
                var selectedBook = dg_books.SelectedItem as BookVM;
                var bookDetail = new BookDetail(selectedBook);

                NavigationService.Navigate(bookDetail);
            }
        }
    }
}
