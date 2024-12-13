using System;
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
        private static EmployeeDTO _currentEmp;
        public static EmployeeDTO currentEmp
        {
            get => _currentEmp;
            set
            {
                _currentEmp = value;
                // Tạo một thể hiện của ObservableObject để gọi OnPropertyChanged
                MainViewModel viewModel = new MainViewModel();
                viewModel.OnPropertyChanged(nameof(currentEmp));

                // Gửi thông báo khi currentEmp thay đổi (bạn có thể sử dụng Messenger hoặc các phương thức tương tự)
                Messenger.Default.Send(new EmployeeUpdatedMessage(currentEmp?.EMP_ID ?? 0, currentEmp?.EMP_NAME));
            }
        }
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
        public CustomerViewModel CustomerVM { get; set; }
        public ErrorViewModel ErrorVM { get; set; }
        public MenuViewModel MenuVM { get; set; }
        public TableViewModel TableVM { get; set; }
        public WorkshiftViewModel WorkshiftVM { get; set; }

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
        public ICommand FirstLoadCM { get; set; }
        public ICommand LogOutCommand { get; set; }
        public MainViewModel()
        {
            FirstLoadCM = new RelayCommand<Window>((p) => { return true; }, (p) => { CurrentName = currentEmp == null ? "" : currentEmp.EMP_NAME; });

            CustomerVM = new CustomerViewModel();
            ErrorVM = new ErrorViewModel();
            MenuVM = new MenuViewModel();
            TableVM = new TableViewModel();
            WorkshiftVM = new WorkshiftViewModel();

            CurrentView = WorkshiftVM;

            CustomerViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = CustomerVM; });
            ErrorViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = ErrorVM; });
            MenuViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = MenuVM; });
            TableViewCommand = new RelayCommand<ContentControl>((p) => {  return true; }, (p) => { CurrentView = TableVM; });
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
