using QuanLiCoffeeShop.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    public partial class ThongKeViewModel : BaseViewModel
    {
        //private static List<ProductDTO> favorList;
        private List<ProductDTO> _favorList;
        internal List<ProductDTO> FavorList
        {
            get { return _favorList; }
            set { _favorList = value; OnPropertyChanged(); }
        }
    }
}
