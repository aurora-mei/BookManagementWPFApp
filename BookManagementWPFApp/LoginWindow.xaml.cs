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
using System.Windows.Shapes;
using BookManagement.DataAccess.Repositories;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IUserRepository _customerRepo;
        public LoginWindow()
        {
            InitializeComponent();
            _customerRepo = new UserRepository();
        }

        private void btn_logIn_Click(object sender, RoutedEventArgs e)
        {
            if (txt_email.Text == " " && txt_password.Password == " ")
            {
                MessageBox.Show("Please enter email and password");
            }
            else
            {
                var role = _customerRepo.Login(txt_email.Text, txt_password.Password);
                if (role == "Admin")
                {
                    AdminWindow adminDashboard = new AdminWindow();
                    adminDashboard.Show();
                    this.Close();
                }
                else if (role == "Invalid")
                {
                    MessageBox.Show("Invalid email or password");
                    return;

                }
                else if (role == "Banned")
                {
                    MessageBox.Show("This account is banned. Please try logging in with a different account!");
                    return;
                }
                else
                {
                    Application.Current.Properties["UserID"] = role;
                    Application.Current.Properties["UserName"] = _customerRepo.GetCustomerByEmail(txt_email.Text).Username;
                    UserWindow userWindow = new UserWindow();
                    userWindow.Show();
                    this.Close();
                }

            }
        }
        private void tb_signUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SignupWindow signUpWindow = new SignupWindow();
            signUpWindow.Show();
            this.Close();
        }
    }
}
