using BookManagement.BusinessObjects;
using BookManagement.BusinessObjects.ViewModel;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Constants;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private readonly IUserRepository _userRepository;
        private readonly ILoanRepository _loanRepository;
        public Order PendingOrder { get; set; }
        private User _user;

        public HomePage()
        {
            InitializeComponent();
            _bookRepository = new BookRepository();
            _mapper = new MyMapper();
            _orderRepository = new OrderRepository();
            _orderItemRepository = new OrderItemRepository();
            _userRepository = new UserRepository();
            _loanRepository = new LoanRepository();
            LoadBooks();
            LoadPendingOrder();
            string userIdString = Application.Current.Properties["UserID"].ToString();
            int userId = Int32.Parse(userIdString);
            _user = _userRepository.GetUser(u => u.UserID == userId);
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

        private void LoadPendingOrder()
        {
            var currentUserId = int.Parse(Application.Current.Properties["UserID"].ToString());
            var hasOrderPending =
                _orderRepository.GetOrder(x => x.UserID == currentUserId && x.Status == MyConstants.STATUS_NOT_PAID);
            if (hasOrderPending != null)
            {
                PendingOrder = hasOrderPending;
            }
            else
            {
                PendingOrder = new Order()
                {
                    UserID = currentUserId,
                    OrderDate = DateTime.Now,
                    ShippingMethod = "Normal delivery",
                    Status = MyConstants.STATUS_NOT_PAID,
                    TotalPrice = 0, // Add later
                };
                _orderRepository.AddOrder(PendingOrder);
            }

            tb_pendingOrderID.Text = PendingOrder.OrderID.ToString();
            var OrderItemVMs = new List<OrderItemVM>();
            if (PendingOrder.OrderItems != null && PendingOrder.OrderItems.Count() > 0)
            {
                foreach (var orderItem in PendingOrder.OrderItems)
                {
                    var orderItemVM = new OrderItemVM();
                    _mapper.Map(orderItem, orderItemVM);
                    OrderItemVMs.Add(orderItemVM);
                }
                ic_orderItems.ItemsSource = new ObservableCollection<OrderItemVM>(OrderItemVMs).ToList<OrderItemVM>();
                tb_totalPrice.Text = "Total: " + PendingOrder.TotalPrice.ToString("C");
            }
        }

        private void btn_viewOrders_Click(object sender, RoutedEventArgs e)
        {
            MyOrderWindow myOrderWindow = new MyOrderWindow(_user);
            Window.GetWindow(this).Close();
            myOrderWindow.Show();
        }


        private void btn_addCart_Click(object sender, RoutedEventArgs e)
        {
            //check xem co order pending nao khong
            //co thi lay, không thì tạo mới
            //sau do load ra
            if (sender is Button b)
            {
                // Get the selected book and it's amount
                var selectedBook = _bookRepository.GetBookById(int.Parse(b.Tag.ToString()));
                if (selectedBook == null) throw new Exception("Please select a book");
                var quantity = 1;
                var discount = selectedBook.Discount;
                var totalPrice = selectedBook.Price * quantity;
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
                    OrderID = PendingOrder.OrderID,
                    BookID = selectedBook.BookID,
                    Price = totalPrice,
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

            LoadPendingOrder();
        }

        private void icon_deleteOrderItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is PackIcon pi && pi.DataContext is OrderItemVM orderItemVM)
            {
                var result = MessageBox.Show("Are you sure you want to remove this item", "Confirm Remove Item",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _orderItemRepository.DeleteOrderItem(new OrderItem()
                    {
                        OrderID = orderItemVM.OrderID,
                        BookID = orderItemVM.BookID
                    });
                    MessageBox.Show("Remove item successfull!");
                }

                MessageBox.Show("Cancel remove action successfull!");
            }
        }

        private void Btn_borrow_OnClick(object sender, RoutedEventArgs e)
        {
            var book = new Book();
            if (sender is Button b)
            {
                book = _bookRepository.GetBookById(int.Parse(b.Tag.ToString()));
            }

            var currentUserId = int.Parse(Application.Current.Properties["UserID"].ToString());
            var currentUser = _userRepository.GetUser(u => u.UserID == currentUserId);
            if (book == null || currentUser == null) return;
            var currentBookLoans = _loanRepository.GetLoan(l => l.BookID == book.BookID);
            var userLoan = currentBookLoans.FirstOrDefault(l => l.UserID == currentUserId);
            if (userLoan != null)
            {
                MessageBox.Show("You have already loan this book");
                return;
            }
            switch (currentBookLoans.Count)
            {
                case < 5:
                    {
                        BookID = book.BookID,
                        UserID = currentUserId,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(5),
                        Status = LoanStatusConstant.Borrowed,
                        FineAmount = book.Price * 25 / 100
                    };
                    _loanRepository.AddLoan(newLoan);
                    MessageBox.Show("Borrowed book successfully! Please remember to it will be automatically returned in 5 days");
                    break;
                }
                case >= 5 when currentBookLoans[0].DueDate < DateTime.Now:
                    currentBookLoans[0].UserID = currentUserId;
                    currentBookLoans[0].BorrowDate = DateTime.Now;
                    currentBookLoans[0].DueDate = DateTime.Now.AddDays(5);
                    MessageBox.Show("Borrowed book successfully! Please remember to it will be automatically returned in 5 days");
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
                    MessageBox.Show("Borrowed book successfully! Please remember to it will be automatically returned in 5 days");
                    break;
            }
        }
    }
}