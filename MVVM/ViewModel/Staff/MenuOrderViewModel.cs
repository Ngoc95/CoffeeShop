using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.ProductCard;
using QuanLiCoffeeShop.MVVM.View.Staff.StaffOrderMenu;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static MaterialDesignThemes.Wpf.Theme;

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

        private ObservableCollection<Bill_InfoDTO> _Bill_InforList;
        public ObservableCollection<Bill_InfoDTO> Bill_InforList
        {
            get { return _Bill_InforList; }
            set 
            { 
                _Bill_InforList = value;
                OnPropertyChanged(nameof(Bill_InforList)); 
            }
        }

        private Bill_InfoDTO _SelectedBillInfor;
        public Bill_InfoDTO SelectedBillInfor
        {
            get { return _SelectedBillInfor; }
            set { _SelectedBillInfor = value; OnPropertyChanged(); }
        }

        private Nullable<decimal> _Total_Bill;
        public Nullable<decimal> Total_Bill
        {
            get { return _Total_Bill; }
            set { _Total_Bill = value; OnPropertyChanged(); }
        }
        private bool _UsePointBtnChecked;
        public bool UsePointBtnChecked
        {
            get { return _UsePointBtnChecked; }
            set { _UsePointBtnChecked = value; OnPropertyChanged(nameof(UsePointBtnChecked)); }
        }

        //diem dang dung
        private Nullable<decimal> _PointHaveUsed;
        public Nullable<decimal> PointHaveUsed
        {
            get { return _PointHaveUsed; } 
            set { _PointHaveUsed = value; OnPropertyChanged(); }
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
        public ICommand AddPrdToBillCommand { get; set; }
        public ICommand SelectProductCommand { get; set; }
        public ICommand DeleteBillInfoCommand { get; set; }
        public ICommand ToggerBtnUsePointCommand { get; set; }
        public ICommand DeleteCurrentBillCommand { get; set; }
        public ICommand SaveCurrAndGenNewBillCommand { get; set; }

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
                UsePointBtnChecked = false;
                CaculatePoint();
                SelectedCustomer = new CustomerDTO(SelectedCustomer);
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

            btnSetDefaultCustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            { 
                UsePointBtnChecked = false;
                CaculatePoint();
                SelectedCustomer = new CustomerDTO() { ID = 0, Point = 0, Name = "Vãng lai" };
            });


            AddPrdToBillCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if(Bill_InforList == null)
                    Bill_InforList = new ObservableCollection<Bill_InfoDTO>();
                if (SelectedBillPrd != null && QuantityOfSelectedProduct != 0 && IdOfNextBill != 0)
                {
                    if(CanAddToBillInforList())
                        Bill_InforList.Add(new Bill_InfoDTO(SelectedBillPrd, QuantityOfSelectedProduct, IdOfNextBill));
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Sản phẩm đã được thêm vào Bill trước đó");
                        return;
                    }
                }
                CaculateTotalBill();
                SelectedBillPrd = null;
                QuantityOfSelectedProduct = 1;
            });

            DeleteBillInfoCommand = new RelayCommand<Bill_InfoDTO>((p) => { return true; }, (p) =>
            {
                if (p != null && Bill_InforList.Contains(p))
                {
                    Bill_InforList.Remove(p);
                    CaculateTotalBill();
                }
            });


            ToggerBtnUsePointCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UsePointBtnChecked = !UsePointBtnChecked;
                CaculatePoint();
                Total_Bill -= PointHaveUsed;
                SelectedCustomer = new CustomerDTO(SelectedCustomer);
            });

            DeleteCurrentBillCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UsePointBtnChecked = false;
                CaculatePoint();
                SelectedCustomer = new CustomerDTO(SelectedCustomer);
                Bill_InforList = new ObservableCollection<Bill_InfoDTO>();
                CaculateTotalBill();
            });

            SaveCurrAndGenNewBillCommand = new RelayCommand<object>((p) => { return true; },async (p) =>
            {

            });
        }

        private void saveBill()
        {

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

        private bool CanAddToBillInforList()
        {
            if(SelectedBillPrd == null)
                return false;
            if (Bill_InforList == null)
                return false;
            foreach(var item in Bill_InforList)
            {
                if (item.PRO_ID == SelectedBillPrd.PRO_ID)
                    return false;
            }
            return true;
        }

        private void CaculateTotalBill()
        {
            if (Bill_InforList == null)
                Total_Bill = 0;
            else
            {
                Total_Bill = 0;
                foreach(var item in Bill_InforList)
                {
                    Total_Bill += item.Total_PRICE_ITEM;
                }
                if(Total_Bill != 0)
                {
                    CaculatePoint();
                    Total_Bill -= PointHaveUsed;
                }
            }
        }

        private void CaculatePoint()
        {
            if (PointHaveUsed == null)
                PointHaveUsed = new decimal();
            
            if(UsePointBtnChecked)
            {
                if(Total_Bill >= SelectedCustomer.Point)
                {
                    PointHaveUsed = SelectedCustomer.Point;
                    SelectedCustomer.Point = 0;
                }
                else
                {
                    PointHaveUsed = SelectedCustomer.Point - Total_Bill;
                    SelectedCustomer.Point -= PointHaveUsed;
                }
            }
            else
            {
                if(PointHaveUsed != 0)
                {
                    Total_Bill += PointHaveUsed;
                    SelectedCustomer.Point += PointHaveUsed;
                }
                PointHaveUsed = 0;
            }
        }
    }
}
