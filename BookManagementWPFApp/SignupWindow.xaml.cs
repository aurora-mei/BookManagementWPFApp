using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        private readonly IUserRepository _userRepo;

        public SignupWindow()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
        }


        private void btn_signUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_email.Text == " " || txt_password.Password == " " || txt_fullName.Text == "" ||
                    txt_confirmPassword.Password == "" || dp_dob.Text == "")
                {
                    MessageBox.Show("Please fill all fields");
                    return;
                }

                if (txt_password.Password != txt_confirmPassword.Password)
                {
                    MessageBox.Show("Password and confirmation do not match. Please try again!");
                    return;
                }
                if (!new EmailAddressAttribute().IsValid(txt_email.Text))
                {
                    MessageBox.Show("Please enter a valid email address");
                    return;
                }
                DateTime dateOfBirth;
                if (!DateTime.TryParse(dp_dob.Text, out dateOfBirth) || dateOfBirth >= DateTime.Now)
                {
                    MessageBox.Show("Please select a valid date of birth in the past");
                    return;
                }

                var inUseEmails = _userRepo.ListUsers().Select(x => x.Email).Distinct()
                    .ToList();
                if (inUseEmails.Any(x => x.Equals(txt_email.Text)))
                {
                    MessageBox.Show("Email address is already in use. Please enter a different email address.");
                    return;
                }
     
                _userRepo.AddUser(new User()
                {
                    Username = txt_fullName.Text,
                    Password = txt_password.Password,
                    Dob = DateOnly.FromDateTime(dateOfBirth),
                    Email = txt_email.Text,
                });
                MessageBox.Show("Thank you for signing up");
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to sign up because: " + ex.Message);
                return;
            }
        }
        private void tb_logIn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

    }
}
