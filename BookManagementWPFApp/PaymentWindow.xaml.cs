using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Constants;
using BookManagementWPFApp.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace BookManagementWPFApp
{
    public partial class PaymentWindow : Window
    {
        // Fix later
        private class OrderDetailVM
        {
            public int OrderId { get; set; }
            public string BookName { get; set; }
            public string Author { get; set; }
            public int Quantity { get; set; }
            public string Desc { get; set; }
            public double UnitPrice { get; set; }
            public double TotalPrice { get; set; }
        }

        private readonly PaymentHelper _paymentHelper;
        private IOrderRepository _orderRepository = new OrderRepository();
        private Order? _order;

        public PaymentWindow()
        {
            InitializeComponent();
            _paymentHelper = new PaymentHelper();
            LoadData();
        }

        // You should pass an in to this page so that it can load the order data
        public PaymentWindow(Order order = null)
        {
            InitializeComponent();
            _order = order;
            _paymentHelper = new PaymentHelper();
            LoadData();
        }

        private async void LoadData()
        {
            // TODO: DEBUG ONLY
            if (_order == null)
            {
                _order = await _orderRepository.GetOrderAsync(o => o.OrderID == 1);
            }
            else
            {
                _order = await _orderRepository.GetOrderAsync(o => o.OrderID == _order.OrderID);
            }

            // Load it to the data grid
            IEnumerable<OrderDetailVM> orderDetails = from o in _order.OrderItems
                select new OrderDetailVM
                {
                    OrderId = o.OrderID,
                    Author = o.Book.Author.authorName,
                    BookName = o.Book.Title,
                    Desc = o.Book.Description,
                    TotalPrice = o.Price,
                    UnitPrice = o.Book.Price,
                    Quantity = o.Quantity,
                };
            dgr_temp.ItemsSource = new ObservableCollection<OrderDetailVM>(orderDetails);
            var previousSelectedIndex = cb_delivery.SelectedIndex;
            cb_delivery.ItemsSource = new[] { "Normal delivery", "Fast delivery" };
            cb_delivery.SelectedIndex = previousSelectedIndex == -1 ? 0 : previousSelectedIndex;
            string orderStatus = "";
            if (_order != null && _order.Status == MyConstants.STATUS_NOT_PAID)
            {
                orderStatus = "Not paid";
            }
            else if (_order.Status == MyConstants.STATUS_PENDING)
            {
                orderStatus = "Waiting for order confirmation";
            }
            else if (_order.Status == MyConstants.STATUS_PAID_AND_CONFIRMED)
            {
                orderStatus = "Paid";
            }

            txt_orderStatus.Content = $"Order status: {orderStatus}";
            txt_orderId.Content = $"Order ID: {_order.OrderID}";
        }

        private async void btn_payClick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the currently select option in combobox
                var deliveryType = cb_delivery.SelectedValue.ToString();
                if (string.IsNullOrEmpty(deliveryType)) deliveryType = "Normal delivery";
                _order.ShippingMethod = deliveryType;
                // Create request based on the order
                var requestCreateDto = _paymentHelper.CreateOrderRequestDto(_order);
                // Get the creation order response
                var response = await _paymentHelper.SendCreateOrderRequest(requestCreateDto);
                // Navigate user to the browser
                var approveLink = response.links.FirstOrDefault(l => l.rel.ToLower() == MyConstants.APPROVE.ToLower())
                    .href;
                var captureLink = response.links.FirstOrDefault(l => l.rel.ToLower() == MyConstants.CAPTURE.ToLower())
                    .href;
                System.Diagnostics.Process.Start(new ProcessStartInfo(approveLink) { UseShellExecute = true });
                // update the order status after all things
                _order.Status = MyConstants.STATUS_PENDING;
                _orderRepository.UpdateOrder(_order);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error has occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadData();
            }
        }

        private async void btn_confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            // Confirm the order of id 1 (Test only)
            try
            {
                if (_order.Status != MyConstants.STATUS_PAID_AND_CONFIRMED)
                {
                    var result =
                        await _paymentHelper.ConfirmOrder(_order.OrderID,
                            _orderRepository);
                    if (result)
                    {
                        MessageBox.Show("Order confirmed, thank you for your purchase", "Confirmation",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("This order has already been paid", "Confirmation", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error has occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadData();
            }
        }

        private void btn_goBack_Click(object sender, RoutedEventArgs e)
        {
            MyOrderWindow myOrderWindow = new MyOrderWindow(_order.User);
            myOrderWindow.Show();
            this.Close();
        }
    }
}