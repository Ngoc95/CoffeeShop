using QuanLiCoffeeShop.MVVM.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiCoffeeShop.MVVM.View.Admin.EmployeeManagement
{
    public partial class EditEmployeeWindow : Window
    {
        public EditEmployeeWindow()
        {
            InitializeComponent();
        }
        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = this.DataContext as EmployeeViewModel;
                if (viewModel != null)
                {
                    viewModel.EditEmp.EMP_PASSWORD = passwordBox.Password;
                }
            }
        }

        private void moveAddEmpWin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void passAddEmp_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tbSDT_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }
        private void tbCCCD_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }
        //hàm ktra text là số
        private bool IsTextNumeric(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]+$");
        }
    }
}
