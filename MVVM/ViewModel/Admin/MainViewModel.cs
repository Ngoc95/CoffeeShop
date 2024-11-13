using QuanLiCoffeeShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand CustomerViewCommand { get; set; }
        public RelayCommand EmployeeViewCommand { get; set; }
        public CustomerViewModel CustomerVM {  get; set; }
        public EmployeeViewModel EmployeeVM { get; set; }

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
           EmployeeVM = new EmployeeViewModel();

           CurrentView = EmployeeVM;

            CustomerViewCommand = new RelayCommand(o =>
            {
                CurrentView = CustomerVM;
            });

            EmployeeViewCommand = new RelayCommand(o =>
            {
                CurrentView = EmployeeVM;
            });
        }
    }
}
