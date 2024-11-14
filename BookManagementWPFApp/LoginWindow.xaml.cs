using System.Windows;
using System.Windows.Input;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btn_logIn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void tb_signUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Navigating to the Sign-Up page...");
        }
    }
}
