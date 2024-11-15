using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Constants;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for BookListWindow.xaml
    /// </summary>
    public partial class BookListWindow : Window
    {
        private IBookRepository _bookRepository = new BookRepository();
        private IUserRepository _userRepository = new UserRepository();
        private IOrderRepository _orderRepository = new OrderRepository();
        private IOrderItemRepository _orderItemRepository = new OrderItemRepository();
        private Order currentOrder = null;
        private User user = null;

        public BookListWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public BookListWindow(User user = null)
        {
            InitializeComponent();
            this.user = user;
            LoadData();
        }

        //Assume that the user id is loaded from the constructor:
        private void LoadData()
        {
            if (user == null)
            {
                user = _userRepository.ListUsers()
                    .FirstOrDefault(u => u.UserID == 1); // TODO: REPLACE THIS LINE WITH PASSED USER ID PARAMS
            }

            // Load the books from the database
            var books = _bookRepository.GetListBooks();
            dgr_books.ItemsSource = new ObservableCollection<Book>(books);
        }

        private void btn_addToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var amount = txt_amount.Text;
                if (string.IsNullOrEmpty(amount) || amount.All(char.IsLetter))
                {
                    MessageBox.Show("Please enter the number of books you want to add to the cart.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    // Create an order for user if it is empty
                    if (currentOrder == null)
                    {
                        currentOrder = new Order()
                        {
                            UserID = user.UserID, // REPLACE THIS WILL CURRENT USER ID
                            OrderDate = DateTime.Now,
                            ShippingMethod = "Normal delivery",
                            Status = MyConstants.STATUS_NOT_PAID,
                            TotalPrice = 0, // Add later
                        };
                        _orderRepository.AddOrder(currentOrder);
                    }

                    // Get the selected book and it's amount
                    var selectedBook = dgr_books.SelectedItem as Book;
                    if (selectedBook == null) throw new Exception("Please select a book");
                    var quantity = int.Parse(amount);
                    var discount = selectedBook.Discount;
                    var totalPrice = selectedBook.Price;
                    if (discount != null)
                    {
                        // 5 quyen 20k => Giam 0.5 => moi quyen 10k
                        var singleDiscountEachBook = selectedBook.Price * discount.discountValue;
                        // 20k * 5 - (10k * 5)
                        totalPrice = selectedBook.Price * quantity - singleDiscountEachBook * quantity;
                    }
                    Console.WriteLine(totalPrice);
                    // Add to our existing order
                    var orderItem = new OrderItem()
                    {
                        OrderID = currentOrder.OrderID,
                        Price = totalPrice,
                        BookID = selectedBook.BookID,
                        Quantity = quantity,
                    };
                    _orderItemRepository.AddOrderItem(orderItem);
                    // Statistics: Update the book information
                    selectedBook.Quantity -= quantity;
                    selectedBook.VisitedNumber += quantity;
                    _bookRepository.UpdateBook(selectedBook);

                    MessageBox.Show($"Added book {selectedBook.Title} to cart.", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                LoadData();
            }
        }

        private void btn_viewOrders_Click(object sender, RoutedEventArgs e)
        {
            var myOrderWindow = new MyOrderWindow(user);
            myOrderWindow.Show();
            this.Close();
        }
    }
}