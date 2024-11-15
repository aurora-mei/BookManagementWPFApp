using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Windows;
using BookManagementWPFApp.Constants;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for MyOrderWindow.xaml
    /// </summary>
    public partial class MyOrderWindow : Window
    {
        private User user = null;
        private IOrderRepository _orderRepository = new OrderRepository();

        public MyOrderWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            LoadData();
        }

        private void LoadData()
        {
            var myOrders = _orderRepository.ListOrders()
                .Where(o => o.UserID == user.UserID);
            dgr_orders.ItemsSource = new ObservableCollection<Order>(myOrders);
        }

        private void btn_payOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = dgr_orders.SelectedItem as Order;
            if (selectedOrder == null)
            {
                MessageBox.Show("Please select an order to pay.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            if (selectedOrder.Status == MyConstants.STATUS_PAID_AND_CONFIRMED)
            {
                MessageBox.Show("You already paid for this order.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            var paymentWindow = new PaymentWindow(selectedOrder);
            paymentWindow.Show();
            this.Close();
        }

        private void btn_backToBooks_Click(object sender, RoutedEventArgs e)
        {
            var bookListWindow = new BookListWindow(user);
            this.Close();
            bookListWindow.Show();
        }
    }
}