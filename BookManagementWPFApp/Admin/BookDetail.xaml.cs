using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Admin.VM;
using Microsoft.Win32;
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

namespace BookManagementWPFApp.Admin
{
    /// <summary>
    /// Interaction logic for BookDetail.xaml
    /// </summary>
    public partial class BookDetail : Page
    {

        private BookVM _BookVM;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IBookRepository _bookRepository;
        public BookDetail(BookVM selectedBook)
        {
            _BookVM = selectedBook;
            _bookRepository = new BookRepository();
            _categoryRepository = new CategoryRepository();
            _authorRepository = new AuthorRepository();
            _discountRepository = new DiscountRepository();
            InitializeComponent();
            LoadCB();
            LoadBookDetail();

        }

        private void LoadBookDetail()
        {
            txt_ID.Text = _BookVM.BookID.ToString();
            img_BookImage.Source = new BitmapImage(new Uri(_BookVM.BookImages, UriKind.RelativeOrAbsolute));
            txt_Title.Text = _BookVM.Title;
            txt_Price.Text = _BookVM.Price.ToString();
            txt_Quantity.Text = _BookVM.Quantity.ToString();
            txt_Description.Text = _BookVM.Description;
            txt_Language.Text = _BookVM.Language;
            dp_PublishDate.SelectedDate = _BookVM.PublishDate;
            cb_Category.SelectedValue = _BookVM.CategoryID;
            cb_Author.SelectedValue = _BookVM.AuthorID;
            cb_Discount.SelectedValue = _BookVM.DiscountID;
        }

        private void LoadCB()
        {
            var categories = _categoryRepository.GetListCategories();
            cb_Category.ItemsSource = categories;
            cb_Category.DisplayMemberPath = "CategoryName";
            cb_Category.SelectedValuePath = "CategoryID";
            var authors = _authorRepository.GetListAuthors();
            cb_Author.ItemsSource = authors;
            cb_Author.DisplayMemberPath = "authorName";
            cb_Author.SelectedValuePath = "AuthorID";
            var discounts = _discountRepository.GetListDiscounts();
            cb_Discount.ItemsSource = discounts;
            cb_Discount.DisplayMemberPath = "discountName";
            cb_Discount.SelectedValuePath = "DiscountID";
        }

        private void btn_ChangeImg_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                img_BookImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                _BookVM.BookImages = openFileDialog.FileName; // Cập nhật đường dẫn hình ảnh
            }
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            var newBook = new Book
            {
                BookID = Convert.ToInt32(txt_ID.Text),
                Title = txt_Title.Text,
                Price = Convert.ToDouble(txt_Price.Text),
                Quantity = Convert.ToInt32(txt_Quantity.Text),
                Description = txt_Description.Text,
                Language = txt_Language.Text,
                PublishDate = dp_PublishDate.SelectedDate.Value,
                CategoryID = (cb_Category.SelectedItem as Category).CategoryID,
                AuthorID = (cb_Author.SelectedItem as Author).AuthorID,
                DiscountID = (cb_Discount.SelectedItem as Discount).DiscountID,
                BookImages = _BookVM.BookImages
            };

            _bookRepository.UpdateBook(newBook);
            MessageBox.Show("Book updated successfully!");
            LoadBookDetail();
        }
    }
}
