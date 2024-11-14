using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Admin.VM;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BookManagementWPFApp.Admin
{
    /// <summary>
    /// Interaction logic for AddBookDetai.xaml
    /// </summary>
    public partial class AddBookDetail : Page
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICatgoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IDiscountRepository _discountRepository;

        public AddBookDetail()
        {
            _bookRepository = new BookRepository();
            _categoryRepository = new CategoryRepository();
            _authorRepository = new AuthorRepository();
            _discountRepository = new DiscountRepository();
            InitializeComponent();
            LoadBook();
            LoadCB();
        }

        public void LoadBook()
        {
            var random = new Random();

            var titles = new List<string>
            { "How to become a mid lane like Chovy", "Play Tristana like Chovy", 
                "How to not choke at Worlds", "How to farm 200+ cs in 15 mins",
                "How to get 3rd Worlds Champion for GenG", "Ryze R + Flash combo" };
            txt_Title.Text = titles[random.Next(titles.Count)];

            txt_Price.Text = random.Next(50000, 500000).ToString();
            txt_Quantity.Text = random.Next(1, 100).ToString();

            var descriptions = new List<string> { "Best of GenG 2022", "Best of GenG 2023", "Best of GenG 2024" };
            txt_Description.Text = descriptions[random.Next(descriptions.Count)];

            var languages = new List<string> { "English", "Vietnamese", "Korea" };
            txt_Language.Text = languages[random.Next(languages.Count)];

        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            string title = txt_Title.Text;
            double price = Convert.ToDouble(txt_Price.Text);
            int quantity = Convert.ToInt32(txt_Quantity.Text);
            string description = txt_Description.Text;
            string language = txt_Language.Text;
            int category = (cb_Category.SelectedItem as Category).CategoryID;
            int author = (cb_Author.SelectedItem as Author).AuthorID;
            int discount = (cb_Discount.SelectedItem as Discount).DiscountID;

            string bookPDFPath = txt_PdfPath.Text;
            string thumbnailPath = img_thumbnailImage.Source.ToString();

            var newBook = new Book
            {
                Title = title,
                Price = price,
                Quantity = quantity,
                Description = description,
                Language = language,
                CategoryID = category,
                AuthorID = author,
                DiscountID = discount,
                BookPDFLink = bookPDFPath, 
                BookImages = thumbnailPath
            };

            _bookRepository.AddBook(newBook);
            MessageBox.Show("Book added successfully!");
            NavigationService.Navigate(new Home());
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

        private void btn_ChoosePdfButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (fileDialog.ShowDialog() == true)
            {
                string booksDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books");

                string bookPath = Path.Combine(booksDirectory, Path.GetFileName(fileDialog.FileName));

                int count = 1;
                while (File.Exists(bookPath))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileDialog.FileName);
                    string extension = Path.GetExtension(fileDialog.FileName);
                    string newFileName = $"{fileNameWithoutExtension}_{count}{extension}";
                    bookPath = Path.Combine(Directory.GetCurrentDirectory(), "Books", newFileName);
                    count++;
                }

                File.Copy(fileDialog.FileName, bookPath);

                txt_PdfPath.Text = bookPath;
                MessageBox.Show("PDF file uploaded successfully!");
            }
        }

        private void btn_ChooseThumbnail_Click(object sender, RoutedEventArgs e)
        {
            var thumbnailDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "PNG Files (*.png)|*.png"
            };

            if (thumbnailDialog.ShowDialog() == true)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                int count = 1;

                string thumbnailPath = Path.Combine(path, Path.GetFileName(thumbnailDialog.FileName));
                while (File.Exists(thumbnailPath))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbnailDialog.FileName);
                    string extension = Path.GetExtension(thumbnailDialog.FileName);
                    string newFileName = $"{fileNameWithoutExtension}_{count}{extension}";
                    thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", newFileName);
                    count++;
                }

                File.Copy(thumbnailDialog.FileName, thumbnailPath);
                img_thumbnailImage.Source = new BitmapImage(new Uri(thumbnailPath));

                MessageBox.Show("Thumbnail uploaded successfully!");
            }
        }
    }
}
