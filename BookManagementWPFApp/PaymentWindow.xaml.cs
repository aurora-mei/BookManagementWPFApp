using System.Collections.ObjectModel;
using System.Diagnostics;
using BookManagementWPFApp.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Constants;
using BookManagementWPFApp.Dtos;
using BookManagementWPFApp.Helpers;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BookManagementWPFApp
{
    public partial class PaymentWindow : Window
    {
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
        public PaymentWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            _paymentHelper = new PaymentHelper();
            LoadData();
        }

        private async void LoadData()
        {
            // Get the order
            if (_order == null)
            {
                _order = await _orderRepository.GetOrderAsync(o => o.OrderID == 1);
            }
            // Load it to the data grid
            IEnumerable<OrderDetailVM> orderDetails = from o in _order.OrderItems
                select new OrderDetailVM
                {
                    OrderId = o.OrderID,
                    Author = o.Book.Author.AuthorName,
                    BookName = o.Book.Title,
                    Desc = o.Book.Description,
                    TotalPrice = o.Price,
                    UnitPrice = o.Book.Price,
                    Quantity = o.Quantity,
                };
            dgr_temp.ItemsSource = new ObservableCollection<OrderDetailVM>(orderDetails);
        }

        private async void btn_payClick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create request based on the order
                var requestCreateDto = _paymentHelper.CreateOrderRequestDto(_order);
                // Get the response
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
                var result =
                    await _paymentHelper.ConfirmOrder(_order.OrderID,
                        _orderRepository); // confirm the order with id = 1
                if (result)
                {
                    MessageBox.Show("Order confirmed, thank you for your purchase", "Confirmation", MessageBoxButton.OK,
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
    }
}