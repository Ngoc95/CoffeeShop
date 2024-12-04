using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.MenuManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.ProductCard;
using QuanLiCoffeeShop.MVVM.ViewModel.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public static List<GenreProductDTO> genPrdList;

        private List<string> _genPrdNameList;
        //public List<string> GenPrdNameList
        //{
        //    get { return _genPrdNameList; }
        //    set 
        //    {
        //        if (_genPrdNameList == null)
        //            _genPrdNameList = new List<string>();
        //        _genPrdNameList = value; 
        //        OnPropertyChanged(); 
        //        if (genPrdList != null) 
        //        { 
        //            foreach (var item in genPrdList) 
        //            {
        //                _genPrdNameList.Add(item.GP_NAME); 
        //            } 
        //        }
        //    }
        //}
        private ObservableCollection<GenreProductDTO> _genreProductList;
        public ObservableCollection<GenreProductDTO> GenreProductList
        {
            get { return _genreProductList; }
            set { _genreProductList = value; OnPropertyChanged(); }
        }


        private ProductDTO _SelectedItem;
        public ProductDTO SelectedItem 
        { 
            get=> _SelectedItem; 
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged();
            } 
        }

        private string _SelectedItemGenreName;
        public string SelectedItemGenreName { get => _SelectedItemGenreName; set { _SelectedItemGenreName = value; OnPropertyChanged(); } }


        #region Command
        public ICommand FirstLoadCM { get; set; }
        public ICommand AddProCommand { get; set; }
        public ICommand PrintMenuCommand {  get; set; }
        public ICommand SearchMenuCommand { get; set; }
        public ICommand DeleteProductCommand {  get; set; }
        public ICommand OpenEditProWDCommand { get; set; }
        public ICommand BtnEditProductDataComand { get; set; }



        #endregion
        public MenuViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                if (ProductList != null)
                {
                    prdList = new List<ProductDTO>(ProductList);
                }

                GenreProductList = new ObservableCollection<GenreProductDTO>(await GenreProService.Ins.GetAllGenre());
                if (GenreProductList != null)
                {
                    genPrdList = new List<GenreProductDTO>(GenreProductList);
                    if (_genPrdNameList == null)
                        _genPrdNameList = new List<string>();
                    if (genPrdList != null)
                    {
                        foreach (var item in genPrdList)
                        {
                            _genPrdNameList.Add(item.GP_NAME);
                        }
                    }
                }
                GenreProductList.Insert(0, new GenreProductDTO(0, "Tất cả")); 
            });

            AddProCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddProductWindow wd = new AddProductWindow();
                wd.ShowDialog();
            });

            OpenEditProWDCommand = new RelayCommand<ProductCard>((p) => { return true; }, (p) =>
            {
                _SelectedItem = p.DataContext as ProductDTO;
                SelectedItemGenreName = GetGenreName();
                EditProductWindow wd = new EditProductWindow(_genPrdNameList, GetGenreName());
                //wd.DataContext = _SelectedItem;
                wd.ShowDialog();
            });

            DeleteProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
            });


            BtnEditProductDataComand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                _SelectedItem.GP_ID = GetGenreID();
                (bool IsAdded, string messageAdd) = await ProductService.Ins.EditPrdList(_SelectedItem, _SelectedItem.PRO_ID);
                if (IsAdded)
                {
                    p.Close();
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }
            });
        }

        public static implicit operator MenuViewModel(StaffMenuOrderViewModel v)
        {
            throw new NotImplementedException();
        }


        public string GetGenreName()
        {
            foreach(GenreProductDTO item in GenreProductList)
            {
                if(SelectedItem.GP_ID == item.GP_ID)
                {
                    return item.GP_NAME;
                }
            }
            return null;
        }

        public int GetGenreID()
        {
            foreach (GenreProductDTO item in GenreProductList)
            {
                if (SelectedItemGenreName == item.GP_NAME)
                {
                    return item.GP_ID;
                }
            }
            return 0;
        }
    }
}
