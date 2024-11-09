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
        public CustomerViewModel CustomerVM {  get; set; }

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
           CurrentView = CustomerVM;
        }
    }
}
