using BookManagement.BusinessObjects.ViewModel;
using BookManagement.DataAccess.Repositories;
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
using Util;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for ManagePendingOrders.xaml
    /// </summary>
    public partial class ManageProcessingOrders : Page
    {

        private readonly IOrderRepository _orderRepo;
        private readonly IMyMapper _mapper;
        public ManageProcessingOrders()
        {
            InitializeComponent();
            _orderRepo = new OrderRepository();
            _mapper = new MyMapper();
            LoadPendingOrders();
        }
        private void LoadPendingOrders()
        {
            var processingOrders = _orderRepo.ListOrders()
                                          .Where(x => x.Status.Equals(OrderStatusConstant.Processing))
                                          .Select(order => new OrderVM
                                          {
                                              OrderID = order.OrderID,
                                              OrderTitle = $"Order {order.OrderID}",
                                              TotalPrice = order.OrderItems.Sum(item => item.Quantity * item.Price),
                                              OrderItems = order.OrderItems.Select(orderItem =>
                                              {
                                                  var orderItemVm = new OrderItemVM();
                                                  _mapper.Map(orderItem, orderItemVm);
                                                  return orderItemVm;
                                              }).ToList()
                                          }).ToList();

            ic_orders.ItemsSource = processingOrders;
        }
        private void btn_confirm_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if(sender is Button b)
            {
                var orderToConfirm = _orderRepo.GetOrderById((int)b.Tag);
                if (orderToConfirm == null)
                {
                    MessageBox.Show("No order found!");
                    return;
                }
                orderToConfirm.Status = OrderStatusConstant.Completed;
                _orderRepo.UpdateOrder(orderToConfirm);
            }
        }
    }
}
