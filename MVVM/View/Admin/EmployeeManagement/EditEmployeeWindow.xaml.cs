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
        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnEditEmp.IsEnabled = CheckIfAllFieldsAreFilled();
        }
        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditEmp.IsEnabled = CheckIfAllFieldsAreFilled();
        }

        private void cbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditEmp.IsEnabled = CheckIfAllFieldsAreFilled();
        }
        private bool CheckIfAllFieldsAreFilled()
        {
            return !string.IsNullOrWhiteSpace(tbName.Text)
                    || !string.IsNullOrWhiteSpace(tbSDT.Text)
                    || !string.IsNullOrWhiteSpace(tbEmail.Text)
                    || !string.IsNullOrWhiteSpace(tbCCCD.Text)
                    || !string.IsNullOrWhiteSpace(tbSalary.Text)
                    || !string.IsNullOrWhiteSpace(tbUsername.Text)
                    || !string.IsNullOrWhiteSpace(tbPassword.Text)
                    || dpBDay.SelectedDate.HasValue
                    || dpStartDate.SelectedDate.HasValue
                    || cbRole.SelectedItem != null;
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
