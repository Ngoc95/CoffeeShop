﻿using QuanLiCoffeeShop.MVVM.ViewModel.Staff;
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

namespace QuanLiCoffeeShop.MVVM.View.Staff.AccountManagement
{
    /// <summary>
    /// Interaction logic for AccountUserControl.xaml
    /// </summary>
    public partial class AccountUserControl : UserControl
    {
        public AccountUserControl()
        {
            InitializeComponent();
            this.Loaded += AccountUserControl_Loaded;
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = this.DataContext as AccountViewModel;
                if (viewModel != null)
                {
                    viewModel.OldPassword = passwordBox.Password;
                }
            }
        }

        private void pbNewPasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = this.DataContext as AccountViewModel;
                if (viewModel != null)
                {
                    viewModel.NewPassword = passwordBox.Password;
                }
            }
        }

        private void pbNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = this.DataContext as AccountViewModel;
                if (viewModel != null)
                {
                    viewModel.NewPasswordConfirm = passwordBox.Password;
                }
            }
        }
        private void AccountUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is AccountViewModel viewModel)
            {
                viewModel.ResetPasswordRequested += ResetPasswordFields;
            }
        }
        public void ResetPasswordFields()
        {
            pbPassword.Password = string.Empty;
            pbNewPassword.Password = string.Empty;
            pbNewPasswordConfirm.Password = string.Empty;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
