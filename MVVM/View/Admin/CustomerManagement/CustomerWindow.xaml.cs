using QuanLiCoffeeShop.DTOs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiCoffeeShop.MVVM.View.Admin.CustomerManagement
{
    public partial class CustomerWindow : UserControl
    {
        public CustomerWindow()
        {
            InitializeComponent();
        }

        private void infoBtn_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CustomerViewModel;
            if (viewModel != null)
            {
                viewModel.InfoBillCM.Execute(viewModel.SelectedBill);
            }
        }
        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CustomerViewModel;
            if (viewModel != null)
            {
                viewModel.DeleteBillCM.Execute(viewModel.SelectedBill);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }
        private bool IsTextNumeric(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]+$");
        }
    }
}
