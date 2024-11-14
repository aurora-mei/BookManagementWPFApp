using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
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
    /// Interaction logic for ManageUserPage.xaml
    /// </summary>
    public partial class ManageUserPage : Page
    {
        private readonly IUserRepository _userRepo;
        private readonly IOrderRepository _orderRepo;
        public ManageUserPage()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
            _orderRepo = new OrderRepository();
            LoadUsers();
        }
        private void LoadUsers()
        {
            // Get all orders in the current month with status "Completed"
            var ordersInThisMonth = _orderRepo.ListOrders()
                .Where(x => x.OrderDate.Month == DateTime.Now.Month && x.Status.Equals(OrderStatusConstant.Completed));

            // Group orders by user and count orders per user, selecting only users with more than 5 orders
            var usersWithOrdersInThisMonth = ordersInThisMonth
                .GroupBy(x => x.User)
                .Select(g => new { User = g.Key, OrderCount = g.Count() })
                .Where(x => x.OrderCount > 5)
                .OrderByDescending(x => x.OrderCount)
                .Select(x => x.User)
                .ToList();

            // Get top 10 users based on order count
            var top10Users = usersWithOrdersInThisMonth.Take(10).ToList();

            // Get all users and exclude the top 10 users to get the other users
            var allUsers = _userRepo.ListUsers();
            var otherUsers = allUsers.Except(top10Users).ToList();

            // Bind top 10 users to dg_top10users
            dg_top10users.ItemsSource = new ObservableCollection<User>(top10Users);

            // Bind other users to dg_anotherusers
            dg_anotherusers.ItemsSource = new ObservableCollection<User>(otherUsers);
        }

        private void btn_sendVoucher_Click(object sender, RoutedEventArgs routedEventArgs)
        {

        }
    }
}
