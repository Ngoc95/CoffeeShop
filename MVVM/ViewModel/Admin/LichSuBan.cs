using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLiCoffeeShop.DTOs;
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

        private static List<BillDTO> billList;
        private ObservableCollection<BillDTO> _billList;
        public ObservableCollection<BillDTO> BillList
        {
            get { return _billList; }
            set { _billList = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Bill_InfoDTO> _productList;

        public ObservableCollection<Bill_InfoDTO> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(); }
        }
        private BillDTO _selectedItem;
        public BillDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        private string _cusName;
        public string CusName
        {
            get { return _cusName; }
            set { _cusName = value; OnPropertyChanged(); }
        }
        private string _empName;
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; OnPropertyChanged(); }
        }
        private string _billDate;
        public string BillDate
        {
            get { return _billDate; }
            set { _billDate = value; OnPropertyChanged(); }
        }
        private decimal _billValue;
        public decimal BillValue
        {
            get { return _billValue; }
            set { _billValue = value; OnPropertyChanged(); }
        }
        private decimal _billDiscount;
        public decimal BillDiscount
        {
            get { return _billDiscount; }
            set { _billDiscount = value; OnPropertyChanged(); }
        }
        private string _soLuong;
        public string SoLuong
        {
            get { return _soLuong; }
            set { _soLuong = value; OnPropertyChanged(); }
        }
    }
}
