using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.MVVM.ViewModel.Admin;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    class MainViewModel : ObservableObject
    {
        public ICommand CustomerViewCommand { get; set; }
        public ICommand ErrorViewCommand { get; set; }
        public ICommand MenuViewCommand { get; set; }
        public ICommand TableViewCommand { get; set; }
        public CustomerViewModel CustomerVM { get; set; }
        public ErrorViewModel ErrorVM { get; set; }
        public MenuOrderViewModel MenuOrderVM { get; set; }
        public StaffTableResViewModel TableVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }

        }

        public MainViewModel()
        {
            CustomerVM = new CustomerViewModel();
            ErrorVM = new ErrorViewModel();
            MenuOrderVM = new MenuOrderViewModel();
            TableVM = new StaffTableResViewModel();
            CurrentView = CustomerVM;

            CustomerViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = CustomerVM; });
            ErrorViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = ErrorVM; });
            MenuViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = MenuOrderVM; });
            TableViewCommand = new RelayCommand<ContentControl>((p) => { return true; }, (p) => { CurrentView = TableVM; });
        }
    }
}
