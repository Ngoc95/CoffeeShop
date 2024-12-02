using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.MenuManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.ProductCard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class MenuViewModel : ObservableObject
    {
        public static List<ProductDTO> prdList;
        private ObservableCollection<ProductDTO> _productList;

        public ObservableCollection<ProductDTO> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ProductCard> List;

        public ICommand FirstLoadCM { get; set; }
        public ICommand AddProCommand { get; set; }
        public ICommand PrintMenuCommand {  get; set; }
        public ICommand SearchMenuCommand { get; set; }
        public ICommand DeleteProductCommand {  get; set; }
        public ICommand EditProductCommand { get; set; }
        public MenuViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                if (ProductList != null)
                {
                    prdList = new List<ProductDTO>(ProductList);
                }
            });

            AddProCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddProductWindow wd = new AddProductWindow();
                wd.ShowDialog();
            });

            EditProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                EditProductWindow wd = new EditProductWindow();
                wd.ShowDialog();
            });

            DeleteProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
            });
        }

    }
}
