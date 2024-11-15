using BookManagement.BusinessObjects.ViewModel;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Constants;
using System.Windows;
using System.Windows.Controls;
using Util;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for MyProcessingOrders.xaml
    /// </summary>
    public partial class MyProcessingOrders : Page
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMyMapper _mapper;
        public MyProcessingOrders()
        {
            InitializeComponent();
            _orderRepo = new OrderRepository();
            _mapper = new MyMapper();
            LoadPendingOrders();
        }
        private void LoadPendingOrders()
        {
            var userId = int.Parse(Application.Current.Properties["UserID"].ToString());
            var processingOrders = _orderRepo.ListOrders()
                                          .Where(x => x.Status.Equals(MyConstants.STATUS_PENDING) && x.UserID == userId)
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
    }
}
