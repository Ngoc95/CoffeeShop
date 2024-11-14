﻿using QuanLiCoffeeShop.Core;
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
        public RelayCommand ErrorViewCommand { get; set; }
        public RelayCommand MenuViewCommand { get; set; }
        public RelayCommand TableViewCommand { get; set; }
        public CustomerViewModel CustomerVM {  get; set; }
        public EmployeeViewModel EmployeeVM { get; set; }
        public ErrorViewModel ErrorVM { get; set; }
        public MenuViewModel MenuVM { get; set; }
        public TableViewModel TableVM { get; set; }

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
           ErrorVM = new ErrorViewModel();
           MenuVM = new MenuViewModel();
           TableVM = new TableViewModel();

           CurrentView = EmployeeVM;

            CustomerViewCommand = new RelayCommand(o =>
            {
                CurrentView = CustomerVM;
            });

            EmployeeViewCommand = new RelayCommand(o =>
            {
                CurrentView = EmployeeVM;
            });

            ErrorViewCommand = new RelayCommand(o =>
            {
                CurrentView = ErrorVM;
            });

            MenuViewCommand = new RelayCommand(o =>
            {
                CurrentView = MenuVM;
            });
            TableViewCommand = new RelayCommand(o =>
            {
                CurrentView = TableVM;
            });
        }
    }
}
