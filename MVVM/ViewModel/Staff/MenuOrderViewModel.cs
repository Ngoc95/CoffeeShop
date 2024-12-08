using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.ProductCard;
using QuanLiCoffeeShop.MVVM.View.Staff.StaffOrderMenu;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    class MenuOrderViewModel : ObservableObject
    {
        #region DeclareForProduct
        private ObservableCollection<GenreProductDTO> _GenreProductList;
        public ObservableCollection<GenreProductDTO> GenreProductList
        {
            get { return _GenreProductList; }
            set { _GenreProductList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductDTO> _ProductList;
        public ObservableCollection<ProductDTO> ProductList
        {
            get { return _ProductList; }
            set { _ProductList = value; OnPropertyChanged(); }
        }

        //Use as Combobox ItemSource
        private List<string> _genPrdNameList;
        public List<string> GenPrdNameList
        {
            get => _genPrdNameList;
            set { _genPrdNameList = value; OnPropertyChanged(); }
        }
        private int _FilterGnereID = 0;
        public int FilterGnereID { get => _FilterGnereID; set { _FilterGnereID = value; OnPropertyChanged(); } }

        private string _SearchText = "";
        public string SearchText { get => _SearchText; set { _SearchText = value; OnPropertyChanged(); } }

        private ProductDTO _SelectedItem;
        public ProductDTO SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _SelectedItemGenreName;
        public string SelectedItemGenreName { get => _SelectedItemGenreName; set { _SelectedItemGenreName = value; OnPropertyChanged(); } }

        #endregion

        #region DeclareForCustomer

        private ObservableCollection<CustomerDTO> _customerList;
        public ObservableCollection<CustomerDTO> CustomerList
        {
            get { return _customerList; }
            set
            {
                _customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }

        private CustomerDTO _SelectedCustomer;
        public CustomerDTO SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set { _SelectedCustomer = value; OnPropertyChanged(); }
        }

        private CustomerDTO _SearchingCustomer;
        public CustomerDTO SearchingCustomer
        {
            get { return _SearchingCustomer; }
            set { _SearchingCustomer = value; OnPropertyChanged(); }
        }

        private string _SearchCustomerIDstring;
        public string SearchCustomerIDstring
        {
            get { return _SearchCustomerIDstring; }
            set { _SearchCustomerIDstring = value; OnPropertyChanged(); }
        }

        private int _SearchCustomerID;
        public int SearchCustomerID
        {
            get { return _SearchCustomerID; }   
            set { _SearchCustomerID = value; OnPropertyChanged(); }
        }

        private string _SearchCustomerName;
        public string SearchCustomerName
        {
            get { return _SearchCustomerName; } 
            set { _SearchCustomerName = value;OnPropertyChanged(); }
        }
        private string _SearchCustomerPhone;
        public string SearchCustomerPhone
        {
            get { return _SearchCustomerPhone; }
            set { _SearchCustomerPhone = value;OnPropertyChanged(); }
        }
        #endregion

        #region DeclareForBill

        private int _QuantityOfSelectedProduct;
        public int QuantityOfSelectedProduct
        {
            get { return _QuantityOfSelectedProduct; }
            set { _QuantityOfSelectedProduct = value; OnPropertyChanged(); }
        }

        private DateTime _Today;
        public DateTime Today
        {
            get { return _Today; }
            set { _Today = value; OnPropertyChanged(); }
        }
        private int _IdOfNextBill;
        public int IdOfNextBill
        {
            get { return _IdOfNextBill; }
            set { _IdOfNextBill = value; OnPropertyChanged(); }
        }

        private ProductDTO _SelectedBillPrd;
        public ProductDTO SelectedBillPrd
        {
            get { return _SelectedBillPrd; }
            set { _SelectedBillPrd = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BILL_INFO> _Bill_InforList;
        private ObservableCollection<BILL_INFO> Bill_InforList
        {
            get { return _Bill_InforList; }
            set { _Bill_InforList = value; OnPropertyChanged(); }
        }

        #endregion


        #region Command
        public ICommand FirstLoadCM { get; set; }  
        public ICommand FilterCommand { get; set; }
        public ICommand OpenInfoProWDCommand { get; set; }
        public ICommand CloseWDCommand { get; set; }
        public ICommand OpenSearchCusWDCommand { get; set; }
        public ICommand CustomerFilterCommand { get; set; }
        public ICommand btnSelectCustomerCommand { get; set; }
        public ICommand btnSetDefaultCustomerCommand { get; set; }

        //Command use to order
        public ICommand SelectProductCommand { get; set; }

        #endregion

        public MenuOrderViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                if (ProductList == null)
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());

                if (GenreProductList == null)
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
                if(SelectedCustomer == null)
                {
                    SelectedCustomer = new CustomerDTO()
                    {
                        Name = "Vãng lai",
                        ID = 0,
                        Point = 0,
                    };
                }
                else
                {
                    SelectedCustomer.ID = 0;
                    SelectedCustomer.Point = 0;
                    SelectedCustomer.Name = "Vãng lai";
                }
                QuantityOfSelectedProduct = 1;
                Today = DateTime.Now;
                IdOfNextBill = await BillService.Ins.NumOfBill() + 1;
            });

            FilterCommand = new RelayCommand<Object>((p) => { return true; }, async (p) =>
            {
                if (p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;

                if (SearchText == "" && FilterGnereID == 0)
                {
                    ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                    return;
                }
                ProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.FilterPrdList(FilterGnereID, SearchText));
            });

            OpenInfoProWDCommand = new RelayCommand<ProductCardStaff>((p) => { return true; }, (p) =>
            {
                _SelectedItem = new ProductDTO(p.DataContext as ProductDTO);
                SelectedItemGenreName = GetGenreName();
                StaffInforPrdWindow wd = new StaffInforPrdWindow();
                wd.ShowDialog();
            });

            CloseWDCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });

            OpenSearchCusWDCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                SearchCustomerIDstring = null; SearchCustomerName = null; SearchCustomerPhone = null;
                SearchingCustomer = new CustomerDTO(SelectedCustomer);
                CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCus()));
                SearchCustomerWindow wd = new SearchCustomerWindow();
                wd.ShowDialog();
            });

            CustomerFilterCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (int.TryParse(SearchCustomerIDstring, out int temp))
                    SearchCustomerID = temp;
                else SearchCustomerID = 0;
                CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.SearchCus(SearchCustomerID, SearchCustomerName, SearchCustomerPhone));
            });

            btnSelectCustomerCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if(SearchingCustomer != null)
                SelectedCustomer = SearchingCustomer;
                p.Close();
            });


            SelectProductCommand = new RelayCommand<ProductDTO>((p) => { return true; }, (p) =>
            {
                SelectedBillPrd = p;    
            });



        }



        public string GetGenreName()
        {
            foreach (GenreProductDTO item in GenreProductList)
            {
                if (SelectedItem.GP_ID == item.GP_ID)
                {
                    return item.GP_NAME;
                }
            }
            return null;
        }

        public void GetProductName()
        {

        }


    }
}
