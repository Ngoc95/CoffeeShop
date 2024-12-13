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
using System.Xml.Linq;

namespace QuanLiCoffeeShop.MVVM.View.Login
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void forgetPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.ShowDialog();
        }

        private void tbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnLogin.IsEnabled = CheckIfAllFieldsAreFilled();
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = CheckIfAllFieldsAreFilled();
        }
        private bool CheckIfAllFieldsAreFilled()
        {
            return !string.IsNullOrWhiteSpace(tbUsername.Text)
                    && !string.IsNullOrWhiteSpace(pbPassword.Password);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
