using Microsoft.Win32;
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
using System.Windows.Media.Imaging;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class MenuViewModel : ObservableObject
    {
        #region Declare

        private ObservableCollection<ProductDTO> _productList;

        public ObservableCollection<ProductDTO> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(); }
        }

        //Use as Combobox ItemSource
        private List<string> _genPrdNameList;
        public List<string> GenPrdNameList
        {
            get => _genPrdNameList;
            set { _genPrdNameList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GenreProductDTO> _GenreProductList;
        public ObservableCollection<GenreProductDTO> GenreProductList
        {
            get { return _GenreProductList; }
            set { _GenreProductList = value; OnPropertyChanged(); }
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

        private string _SearchText = "";
        public string SearchText { get => _SearchText; set { _SearchText= value; OnPropertyChanged(); } }

        private int _FilterGnereID = 0;
        public int FilterGnereID { get => _FilterGnereID; set { _FilterGnereID = value; OnPropertyChanged(); } }

        private int _IDOfNextProduct = 0;
        public int IDOfNextProduct { get => _IDOfNextProduct; set { _IDOfNextProduct = value; OnPropertyChanged(); } }

        #endregion

        #region Command
        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenAddProWDCommand { get; set; }
        public ICommand OpenEditProWDCommand { get; set; }
        public ICommand PrintMenuCommand {  get; set; }
        public ICommand SearchMenuCommand { get; set; }
        public ICommand DeleteProductCommand {  get; set; }
        public ICommand BtnEditProductDataComand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand BtnImageCommand { get; set; }
        public ICommand BtnAddProductDataComand { get; set; }
        public ICommand CloseWDnotChangeCommand { get; set; }

        #endregion
        public MenuViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {

                 ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());

                if(GenreProductList == null)
                    GenreProductList = new ObservableCollection<GenreProductDTO>(await GenreProService.Ins.GetAllGenre());
                if (GenreProductList != null)
                {
                    if (_genPrdNameList == null)
                    {                        
                        _genPrdNameList = new List<string>();
                        foreach (var item in GenreProductList)
                        {
                            _genPrdNameList.Add(item.GP_NAME);
                        }
                        GenreProductList.Insert(0, new GenreProductDTO(0, "Tất cả"));
                        
                    }
                }
                FilterGnereID = 0;
                SearchText = "";
            });

            OpenAddProWDCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IDOfNextProduct = await ProductService.Ins.NumOfProduct() + 1;
                _SelectedItem = new ProductDTO();
                SelectedItemGenreName = "";
                AddProductWindow wd = new AddProductWindow();
                wd.ShowDialog();
            });

            OpenEditProWDCommand = new RelayCommand<ProductCard>((p) => { return true; }, (p) =>
            {
                _SelectedItem = new ProductDTO(p.DataContext as ProductDTO);
                SelectedItemGenreName = GetGenreName();
                EditProductWindow wd = new EditProductWindow();
                //wd.DataContext = _SelectedItem;
                wd.ShowDialog();
            });

            CloseWDnotChangeCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });


            DeleteProductCommand = new RelayCommand<ProductCard>((p) => { return true; }, async (p) =>
            {
                _SelectedItem = p.DataContext as ProductDTO;
                SelectedItemGenreName = GetGenreName();
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool IsDeleted, string messageDelete) = await ProductService.Ins.DeletePrdList(SelectedItem.PRO_ID);
                    if (IsDeleted)
                    {
                        ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.FilterPrdList(FilterGnereID, SearchText));
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });


            BtnEditProductDataComand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, async (p) =>
            {
                _SelectedItem.GP_ID = GetGenreID();
                (bool IsAdded, string messageAdd) = await ProductService.Ins.EditPrdList(_SelectedItem, _SelectedItem.PRO_ID);
                if (IsAdded)
                {
                    p.Close();
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.FilterPrdList(FilterGnereID, SearchText));
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }
            });

            FilterCommand = new RelayCommand<Object>((p) => { return true; }, async (p) =>
            {
                if(p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;

                if (SearchText == "" && FilterGnereID == 0)
                {
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                    return;
                }
                ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.FilterPrdList(FilterGnereID, SearchText));
            });

            SearchMenuCommand = new RelayCommand<Object>((p) => { return true; }, async (p) =>
            {
                
                if (SearchText == "")
                {
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                }
                else
                {
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.FilterPrdList(FilterGnereID, SearchText));
                }
            });


            BtnImageCommand = new RelayCommand<Image>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.png;*.jpeg;*.gif|All Files|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // Tạo một BitmapImage từ đường 
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Để tải ảnh ngay lập tức
                        bitmap.EndInit();
                        p.Source = bitmap;

                    }
                    catch 
                    {
                        // Hiển thị lỗi nếu quá trình tải ảnh thất bại
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Tải ảnh thất bại");
                    }
                }
            });

            BtnAddProductDataComand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                _SelectedItem.GP_ID = GetGenreID();
                (bool IsAdded, string messageAdd) = await ProductService.Ins.AddPrdList(_SelectedItem);
                if (IsAdded)
                {
                    p.Close();
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.FilterPrdList(FilterGnereID, SearchText));
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }
            });


            PrintMenuCommand = new RelayCommand<Window>((p) => { return true; },(p) =>
            {

            });
        }

        public static implicit operator MenuViewModel(MenuOrderViewModel v)
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
