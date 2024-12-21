﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Login;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.ViewModel.Admin;
using QuanLiCoffeeShop.MVVM.ViewModel.Login;
using static QuanLiCoffeeShop.MVVM.ViewModel.Staff.WorkshiftViewModel;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    class MainViewModel : ObservableObject
    {
        public static EmployeeDTO currentEmp;

        private string _currentName;
        public string CurrentName
        {
            get { return _currentName; }
            set { _currentName = value; OnPropertyChanged(); }
        }

        public ICommand CustomerViewCommand { get; set; }
        public ICommand ErrorViewCommand { get; set; }
        public ICommand MenuViewCommand { get; set; }
        public ICommand TableViewCommand { get; set; }
        public ICommand WorkshiftViewCommand { get; set; }
        public ICommand AccountViewCommand { get; set; }
        public CustomerViewModel CustomerVM { get; set; }
        public ErrorViewModel ErrorVM { get; set; }
        public WorkshiftViewModel WorkshiftVM { get; set; }
        public MenuOrderViewModel MenuOrderVM { get; set; }
        public StaffTableResViewModel TableVM { get; set; }
        public AccountViewModel AccountVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }

        }
        private bool _isAccountSelected;
        public bool IsAccountSelected
        {
            get => _isAccountSelected;
            set
            {
                _isAccountSelected = value;
                OnPropertyChanged(nameof(IsAccountSelected));
            }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand LogOutCommand { get; set; }
        public MainViewModel()
        {
            // Lắng nghe thông báo từ AccountViewModel
            Messenger.Default.Register<PropertyChangedMessage<string>>(this, message =>
            {
                if (message.PropertyName == nameof(CurrentName))
                {
                    CurrentName = message.NewValue;
                }
            });
            FirstLoadCM = new RelayCommand<Window>((p) => { return true; }, (p) => { CurrentName = currentEmp == null ? "" : currentEmp.EMP_NAME; });

            AccountVM = new AccountViewModel();
            CustomerVM = new CustomerViewModel();
            ErrorVM = new ErrorViewModel();
            WorkshiftVM = new WorkshiftViewModel();
            MenuOrderVM = new MenuOrderViewModel();
            TableVM = new StaffTableResViewModel();

            CurrentView = WorkshiftVM;

            AccountViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CustomerViewCommand.Execute(null); IsAccountSelected = true; CurrentView = AccountVM; });
            CustomerViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = CustomerVM; IsAccountSelected = false; });
            ErrorViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = ErrorVM; });
            MenuViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = MenuOrderVM; });
            TableViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = TableVM; });
            WorkshiftViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = WorkshiftVM; });

            LogOutCommand = new RelayCommand<Window>(null, (p) =>
            {
                ConfirmLogOut confirmLogOut = new ConfirmLogOut();
                confirmLogOut.ShowDialog();

                if (confirmLogOut.DialogResult == true)
                {
                    if (p.Owner != null)
                    {
                        LoginWindow newLogin = new LoginWindow();
                        newLogin.Show();
                        p.Owner.Close();
                    }
                    // Đóng cửa sổ hiện tại
                    p.Close();
                }
            });
        }
    }
}
