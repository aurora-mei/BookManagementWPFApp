using BookManagement.BusinessObjects;
using BookManagement.BusinessObjects.ViewModel;
using BookManagement.DataAccess.Repositories;
using MaterialDesignThemes.Wpf;
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
        public Order PendingOrder{get;set;}
        public HomePage()
        {
            InitializeComponent();
            _bookRepository = new BookRepository();
            _mapper = new MyMapper();
            _orderRepository = new OrderRepository();
            _orderItemRepository = new OrderItemRepository();
            LoadBooks();
            LoadPendingOrder();
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

        private  void LoadPendingOrder()
        {
            var currentUserId = int.Parse(Application.Current.Properties["UserID"].ToString());
            var hasOrderPending =  _orderRepository.GetOrder(x => x.UserID == currentUserId && x.Status == OrderStatusConstant.Pending);
            if (hasOrderPending != null)
            {
                PendingOrder = hasOrderPending;
            }
            else
            {
                PendingOrder = new Order()
                {
                    UserID = currentUserId
                };
                _orderRepository.AddOrder(PendingOrder);
            }
            tb_pendingOrderID.Text = PendingOrder.OrderID.ToString();
            var OrderItemVMs = new List<OrderItemVM>();
            if (PendingOrder.OrderItems.Count() > 0)
            {
                foreach (var orderItem in PendingOrder.OrderItems)
                {
                    var orderItemVM = new OrderItemVM();
                    _mapper.Map(orderItem, orderItemVM);
                    OrderItemVMs.Add(orderItemVM);
                }
                ic_orderItems.ItemsSource = new ObservableCollection<OrderItemVM>(OrderItemVMs).ToList<OrderItemVM>();
                tb_totalPrice.Text ="Total: "+PendingOrder.TotalPrice.ToString("C");
            }
        }
        private void btn_addCart_Click(object sender, RoutedEventArgs e)
        {
            //check xem co order pending nao khong
            //co thi lay, không thì tạo mới
            //sau do load ra
            if (sender is Button b)
            {
                var book = _bookRepository.GetBookById(int.Parse(b.Tag.ToString()));
                _orderItemRepository.AddOrderItem(new OrderItem()
                {
                    BookID = (int)b.Tag,
                    OrderID = PendingOrder.OrderID,
                    Price = book.Price,
                    Quantity = 1
                });
            }
            LoadPendingOrder();
        }
        private void nud_updateOrderItem_Click(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           
            if (sender is NumericUpDown nud && nud.DataContext is OrderItemVM orderItemVM)
            {
                _orderItemRepository.UpdateOrderItem(new OrderItem()
                {
                    OrderID = orderItemVM.OrderID,
                    BookID = orderItemVM.BookID,
                    Quantity = nud.Value,
                });
                //tb_quantity.Text = "x" + nud.Value;
            }
        }
        private void icon_deleteOrderItem_Click(object sender, RoutedEventArgs e)
        {
            if(sender is PackIcon pi && pi.DataContext is OrderItemVM orderItemVM)
            {
                var result = MessageBox.Show("Are you sure you want to remove this item", "Confirm Remove Item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
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

        }
    
}
