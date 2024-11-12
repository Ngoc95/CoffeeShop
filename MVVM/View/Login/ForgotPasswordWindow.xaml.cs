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

namespace QuanLiCoffeeShop.MVVM.View.Login
{
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private void tbEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSendmail.IsEnabled = !string.IsNullOrWhiteSpace(tbEmail.Text);
        }
        private void tbNewPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnOk.IsEnabled = !string.IsNullOrWhiteSpace(tbNewPassword.Text);
        }

        private void moveWin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
