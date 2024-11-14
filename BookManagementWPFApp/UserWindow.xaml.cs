using System.Windows;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }

        // Event handler for MenuItem Click event
        private void navigate_logOut(object sender, RoutedEventArgs e)
        {
            // Example: Confirm log out action
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Perform log out actions, such as closing the current window and returning to the login screen
                // Close the main window
                this.Close();

                // Optionally, open a login window
                // var loginWindow = new LoginWindow();
                // loginWindow.Show();
            }
        }
    }
}
