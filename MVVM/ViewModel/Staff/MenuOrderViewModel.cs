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
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static MaterialDesignThemes.Wpf.Theme;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    class MenuOrderViewModel : ObservableObject
    {
        private EmployeeDTO currentEmp;

        #region DeclareForProduct
        private ObservableCollection<GenreProductDTO> _GenreProductList;
        public ObservableCollection<GenreProductDTO> GenreProductList
        {
            get { return _GenreProductList; }
            set { _GenreProductList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductDTO> CoreProductList;

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

        private ObservableCollection<CustomerDTO> CoreCustomerList;

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
        private BillDTO _CurrentBill;
        public BillDTO CurrentBill
        {
            get { return _CurrentBill; }
            set { _CurrentBill = value; OnPropertyChanged(); }
        }
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
        private int _IdOfGenerateBill;
        public int IdOfGenerateBill
        {
            get { return _IdOfGenerateBill; }
            set { _IdOfGenerateBill = value; OnPropertyChanged(); }
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
        private Nullable<decimal> _Discount_Bill;
        public Nullable<decimal> Discount_Bill
        {
            get { return _Discount_Bill; }
            set { _Discount_Bill = value; OnPropertyChanged(); }
        }
        private Nullable<decimal> _SUBTotal_Bill;
        public Nullable<decimal> SUBTotal_Bill
        {
            get { return _SUBTotal_Bill; }
            set { _SUBTotal_Bill= value; OnPropertyChanged(); }
        }
        private Nullable<decimal> _AddPoint = 0;
        public Nullable<decimal> AddPoint
        {
            get { return _AddPoint; }
            set { _AddPoint = value; OnPropertyChanged(); }
        }
        #endregion


        #region Command
        public ICommand FirstLoadCM { get; set; }  
        public ICommand FilterCommand { get; set; }
        public ICommand OpenInfoProWDCommand { get; set; }
        public ICommand OpenSearchCusWDCommand { get; set; }
        public ICommand CustomerFilterCommand { get; set; }
        public ICommand btnSelectCustomerCommand { get; set; }
        public ICommand btnSetDefaultCustomerCommand { get; set; }
        public ICommand ReloadCustomerCommand { get; set; }

        //Command use to order
        public ICommand AddPrdToBillCommand { get; set; }
        public ICommand SelectProductCommand { get; set; }
        public ICommand DeleteBillInfoCommand { get; set; }
        public ICommand ToggerBtnUsePointCommand { get; set; }
        public ICommand DeleteCurrentBillCommand { get; set; }
        public ICommand SaveCurrAndGenNewBillCommand { get; set; }
        public ICommand SaveAndPrintBillCommand { get; set; }

        #endregion

        public MenuOrderViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                if(CoreProductList == null)
                {
                    CoreProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                    ProductList = new ObservableCollection<ProductDTO>(CoreProductList);
                }

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
                if(currentEmp == null)
                    currentEmp = MainViewModel.currentEmp;
                FilterGnereID = 0;
                SearchText = "";
                QuantityOfSelectedProduct = 1;
                Today = DateTime.Now;
                FilterProduct(FilterGnereID, SearchText);
                if(IdOfGenerateBill == 0)
                    IdOfGenerateBill = await BillService.Ins.NumOfBill() + 1;
                SetNewBill();
            });

            FilterCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;
                FilterProduct(FilterGnereID, SearchText);
            });

            OpenInfoProWDCommand = new RelayCommand<ProductCardStaff>((p) => { return true; }, (p) =>
            {
                _SelectedItem = new ProductDTO(p.DataContext as ProductDTO);
                SelectedItemGenreName = GetGenreName();
                StaffInforPrdWindow wd = new StaffInforPrdWindow();
                wd.ShowDialog();
            });

            OpenSearchCusWDCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                UsePointBtnChecked = false;
                CaculatePoint();
                SelectedCustomer = new CustomerDTO(SelectedCustomer);
                SearchCustomerIDstring = ""; SearchCustomerName = ""; SearchCustomerPhone = "";
                SearchingCustomer = new CustomerDTO(SelectedCustomer);
                if(CoreCustomerList == null)
                {
                    CoreCustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCus()));
                }
                CustomerList = new ObservableCollection<CustomerDTO>(CoreCustomerList); 
                SearchCustomerWindow wd = new SearchCustomerWindow();
                wd.ShowDialog();
            });

            CustomerFilterCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (int.TryParse(SearchCustomerIDstring, out int temp))
                    SearchCustomerID = temp;
                else SearchCustomerID = 0;
                FilterCustomer(SearchCustomerID, SearchCustomerName, SearchCustomerPhone);
            });

            btnSelectCustomerCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if(SearchingCustomer != null)
                SelectedCustomer = new CustomerDTO(SearchingCustomer);
                caculateAddPoint();
                p.Close();
            });

            ReloadCustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CustomerList = new ObservableCollection<CustomerDTO>(CoreCustomerList);
                SearchCustomerPhone = "";
                SearchCustomerName = "";
                SearchCustomerIDstring = "";
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
                caculateAddPoint();
            });


            AddPrdToBillCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if(Bill_InforList == null)
                    Bill_InforList = new ObservableCollection<Bill_InfoDTO>();
                if (SelectedBillPrd != null && QuantityOfSelectedProduct != 0 && IdOfGenerateBill != 0)
                {
                    if(CanAddToBillInforList())
                    {
                        Bill_InforList.Add(new Bill_InfoDTO(SelectedBillPrd, QuantityOfSelectedProduct, IdOfGenerateBill));
                        CaculateSubTotalBill();
                        CaculatePoint();
                        if (SUBTotal_Bill != 0)
                        {
                            Total_Bill = SUBTotal_Bill - Discount_Bill;
                            caculateAddPoint();
                        }
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Sản phẩm đã được thêm vào Bill trước đó");
                        return;
                    }
                }
                SelectedBillPrd = null;
                QuantityOfSelectedProduct = 1;
            });

            DeleteBillInfoCommand = new RelayCommand<Bill_InfoDTO>((p) => { return true; }, (p) =>
            {
                if (p != null && Bill_InforList.Contains(p))
                {
                    Bill_InforList.Remove(p);
                    CaculateSubTotalBill();
                    CaculatePoint();
                    if (SUBTotal_Bill != 0)
                    {
                        Total_Bill = SUBTotal_Bill - Discount_Bill;
                        caculateAddPoint();
                    }
                }
            });


            ToggerBtnUsePointCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UsePointBtnChecked = !UsePointBtnChecked;
                if(SelectedCustomer.ID != 0)
                {
                    CaculatePoint();
                    Total_Bill = SUBTotal_Bill - Discount_Bill;
                    caculateAddPoint();
                }
            });

            DeleteCurrentBillCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UsePointBtnChecked = false;
                CaculatePoint();
                Bill_InforList = new ObservableCollection<Bill_InfoDTO>();
                CaculateSubTotalBill();
                Total_Bill = 0;
                caculateAddPoint();
            });

            SaveCurrAndGenNewBillCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if(GenerateCurrentBill())
                {
                    (bool res, string mess) = await BillService.Ins.AddBill(CurrentBill);
                    if(res)
                    {
                        if(SelectedCustomer.ID != 0)
                        {
                            SelectedCustomer.Point += AddPoint;
                            updateCoreCustomerList(SelectedCustomer);
                            (bool a, string b) = await CustomerService.Ins.EditCusPoint(SelectedCustomer, SelectedCustomer.ID);
                            if(!a)
                            {
                                MessageBoxCustom.Show(MessageBoxCustom.Error, b);
                                return;
                            }
                        }
                        bool temp = await GenerateBillInforAndSaveToDB();
                        IdOfGenerateBill++;
                        UsePointBtnChecked = false;
                        Bill_InforList = new ObservableCollection<Bill_InfoDTO>();
                        CaculateSubTotalBill();
                        CaculatePoint();
                        Total_Bill = 0;
                        caculateAddPoint();
                        SelectedCustomer = new CustomerDTO() { ID = 0, Point = 0, Name = "Vãng lai" };
                        if (temp)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Success, "Thêm hóa đơn thành công");
                        }
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, mess);
                    }
                }
            });

            SaveAndPrintBillCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (GenerateCurrentBill())
                {
                    (bool res, string mess) = await BillService.Ins.AddBill(CurrentBill);
                    if (res)
                    {
                    if (SelectedCustomer.ID != 0)
                    {
                        SelectedCustomer.Point += AddPoint;
                        updateCoreCustomerList(SelectedCustomer);
                        (bool a, string b) = await CustomerService.Ins.EditCusPoint(SelectedCustomer, SelectedCustomer.ID);
                        if (!a)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, b);
                            return;
                        }
                    }
                        bool temp = await GenerateBillInforAndSaveToDB();
                        if (temp)
                        {
                            PrintReceipt();
                        }
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra khi lưu vào Database");
                            return;
                        }
                        IdOfGenerateBill++;
                        UsePointBtnChecked = false;
                        Bill_InforList = new ObservableCollection<Bill_InfoDTO>();
                        CaculateSubTotalBill();
                        CaculatePoint();
                        Total_Bill = 0;
                        caculateAddPoint();
                        SelectedCustomer = new CustomerDTO() { ID = 0, Point = 0, Name = "Vãng lai" };
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, mess);
                    }
                }
            });
        }

        private void PrintReceipt()
        {
            FlowDocument document = CreateDocument();
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)  
            {
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Hóa đơn bán hàng");
            }
        }

        private FlowDocument CreateDocument()
        {
            FlowDocument doc = new FlowDocument
            {
                FontFamily = new System.Windows.Media.FontFamily("Tahoma"),
                FontSize = 12,
                PagePadding = new Thickness(10), 
                ColumnWidth = 300, 
                PageWidth = 300
            };

            doc.Blocks.Add(new Paragraph(new Run("CoffeeShop"))
            {
                TextAlignment = TextAlignment.Center
            });
            doc.Blocks.Add(new Paragraph(new Run("Khu phố 6, P.Linh Trung, Tp.Thủ Đức, Tp.HCM\n----------------------"))
            {
                TextAlignment = TextAlignment.Center
            });

            doc.Blocks.Add(new Paragraph(new Run("HÓA ĐƠN BÁN HÀNG"))
            {
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            });
            doc.Blocks.Add(new Paragraph(new Run($"Mã hóa đơn: {IdOfGenerateBill:00000}               {CurrentBill.CREATE_AT:dd/MM/yyyy HH:mm:ss}"))
            {
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Left,
            });
            doc.Blocks.Add(new Paragraph(new Run($"Mã nhân viên: {currentEmp.EMP_ID:000}" ) )
            {
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Left,
            });
            if(SelectedCustomer.ID != 0)
            {
                doc.Blocks.Add(new Paragraph(new Run($"Mã khách hàng: {SelectedCustomer.ID:000}\n{SelectedCustomer.Name}"))
                {
                    Margin = new Thickness(0),
                    FontSize = 13,
                    TextAlignment = TextAlignment.Left,
                });
            }

            doc.Blocks.Add(new Paragraph(new Run("----------------------------------------------------------------"))
            {
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Center,
            });

            Table table = new Table()
            {
                Margin = new Thickness(0),
            };
            table.Columns.Add(new TableColumn { Width = new GridLength(110) });
            table.Columns.Add(new TableColumn { Width = new GridLength(20) });
            table.Columns.Add(new TableColumn { Width = new GridLength(60) });
            table.Columns.Add(new TableColumn { Width = new GridLength(80) });
            table.RowGroups.Add(new TableRowGroup());

            TableRow header = new TableRow();
            header.Cells.Add(new TableCell(new Paragraph(new Run("Tên món"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("SL"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Đ.giá"))) { FontWeight = FontWeights.Bold, TextAlignment = TextAlignment.Right });
            header.Cells.Add(new TableCell(new Paragraph(new Run("T.tiền"))) { FontWeight = FontWeights.Bold, TextAlignment = TextAlignment.Right });
            table.RowGroups[0].Rows.Add(header);

            foreach (var item in Bill_InforList)  
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.PRO_Name))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.QUANTITY.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run($"{item.PRICE_ITEM:C0}")) { TextAlignment = TextAlignment.Right}));
                row.Cells.Add(new TableCell(new Paragraph(new Run($"{item.Total_PRICE_ITEM:C0}")) { TextAlignment = TextAlignment.Right}));
                table.RowGroups[0].Rows.Add(row);
            }

            doc.Blocks.Add(table);
            doc.Blocks.Add(new Paragraph(new Run("-----------------------------"))
            {
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Right,
            });

            doc.Blocks.Add(new Paragraph(new Run($"Tổng giá trị: {SUBTotal_Bill:C0}"))
            {
                Margin= new Thickness(0),
                TextAlignment = TextAlignment.Right
            });
            doc.Blocks.Add(new Paragraph(new Run($"Giảm giá: {Discount_Bill:C0}"))
            {   
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Right
            });
            doc.Blocks.Add(new Paragraph(new Run($"Tổng: {Total_Bill:C0}"))
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right
            });

            doc.Blocks.Add(new Paragraph(new Run("******************************************"))
            {
                TextAlignment = TextAlignment.Center,
            });

            if (SelectedCustomer.ID != 0)
            {
                doc.Blocks.Add(new Paragraph(new Run($"Bạn tích được thêm {AddPoint:,0} điểm"))
                {
                    Margin = new Thickness(0),
                    FontSize = 15,
                    TextAlignment = TextAlignment.Center,
                });
                doc.Blocks.Add(new Paragraph(new Run($"Tổng điểm tích lũy được: {SelectedCustomer.Point:0} điểm"))
                {
                    Margin = new Thickness(0),
                    FontSize = 15,
                    TextAlignment = TextAlignment.Center,
                });
            }
            else
            {

                doc.Blocks.Add(new Paragraph(new Run($"Bạn tích được thêm 0 điểm"))
                {   
                    Margin = new Thickness(0),
                    FontSize = 15,
                    TextAlignment = TextAlignment.Center,
                });
                doc.Blocks.Add(new Paragraph(new Run("Đăng kí tài khoản khách hàng để được tham gia chương trình tích điểm giảm giá!!!"))
                {
                    FontSize = 12,
                    Margin = new Thickness(0),
                    TextAlignment = TextAlignment.Center,
                });
            }

            doc.Blocks.Add(new Paragraph(new Run("CoffeeShop xin cảm ơn và hẹn gặp lại"))
            {
                FontSize = 15,
                TextAlignment = TextAlignment.Center,
            });
            doc.Blocks.Add(new Paragraph(new Run("----------------------------------------------------------------"))
            {
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Center,
            });
            
            return doc;  // Trả về tài liệu để in
        }


        private void updateCoreCustomerList(CustomerDTO selectedCustomer)
        {
            foreach(CustomerDTO item in CoreCustomerList)
            {
                if(item.ID == selectedCustomer.ID)
                {
                    item.Point = selectedCustomer.Point;
                    return;
                }
            }
        }

        private void caculateAddPoint()
        {
            if (SelectedCustomer.ID != 0)
            {
                AddPoint = Total_Bill / 20;
            }
            else AddPoint = null;
        }

        private void SetNewBill()
        {
            CurrentBill = new BillDTO();
            CurrentBill.EMP_ID = currentEmp.EMP_ID;
            CurrentBill.TOTAL_COST = 0;
            CurrentBill.DISCOUNT = 0;
            CurrentBill.SUBTOTAL = 0;
            CurrentBill.CUS_ID = null;
        }

        private async Task<bool> GenerateBillInforAndSaveToDB()
        {
            return await Bill_InfoService.Ins.AddBillInfor(Bill_InforList);
        }

        private void FilterProduct(int filterGenID, string searchtxt)
        {
            ProductList = new ObservableCollection<ProductDTO>();
            if (filterGenID != 0 && searchtxt != "")
            {
                foreach(ProductDTO product in CoreProductList)
                {
                    if(product.PRO_NAME.ToLower().Contains(searchtxt.ToLower()) && product.GP_ID == filterGenID)
                    {
                        ProductList.Add(product);
                    }
                }
            }
            else if (searchtxt == "" && filterGenID != 0)
            {
                foreach (ProductDTO product in CoreProductList)
                {
                    if(product.GP_ID == filterGenID)
                        ProductList.Add(product);
                }
            }
            else if (filterGenID == 0 && searchtxt.Length != 0)
            {
                foreach (ProductDTO product in CoreProductList)
                {
                    if (product.PRO_NAME.ToLower().Contains(searchtxt.ToLower()))
                    {
                        ProductList.Add(product);
                    }
                }
            }
            else
            {
                ProductList = new ObservableCollection<ProductDTO>(CoreProductList);
            }
        }

        private bool GenerateCurrentBill()
        {
            if(Bill_InforList.Count == 0)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Chưa chọn sản phẩm nào!!");
                return false;
            }
            CurrentBill = new BillDTO();
            if(currentEmp == null)
                currentEmp = MainViewModel.currentEmp;
            CurrentBill.EMP_ID = currentEmp.EMP_ID;
            if (SelectedCustomer.ID == 0)
                CurrentBill.CUS_ID = null;
            else
                CurrentBill.CUS_ID = SelectedCustomer.ID;
            CurrentBill.TOTAL_COST = Total_Bill;
            CurrentBill.SUBTOTAL = SUBTotal_Bill;
            CurrentBill.DISCOUNT = Discount_Bill;
            CurrentBill.CREATE_AT = DateTime.Now;
            return true;
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

        private void CaculateSubTotalBill()
        {
            if (Bill_InforList == null)
                SUBTotal_Bill = 0;
            else
            {
                SUBTotal_Bill = 0;
                foreach(var item in Bill_InforList)
                {
                    SUBTotal_Bill+= item.Total_PRICE_ITEM;
                }
            }
        }

        private void CaculatePoint()
        {
            SelectedCustomer.Point += Discount_Bill;
            if(UsePointBtnChecked)
            {
                if(SUBTotal_Bill > SelectedCustomer.Point)
                {
                    Discount_Bill = SelectedCustomer.Point;
                    SelectedCustomer.Point = 0;
                }
                else
                {
                    Discount_Bill = SUBTotal_Bill;
                    SelectedCustomer.Point -= Discount_Bill;
                }
                SelectedCustomer = new CustomerDTO(SelectedCustomer);
            }
            else
            {
                if(Discount_Bill != 0)
                {
                    Total_Bill += Discount_Bill;
                    caculateAddPoint();
                }
                Discount_Bill = 0;
                SelectedCustomer = new CustomerDTO(SelectedCustomer);
            }
        }

        private void FilterCustomer(int ID, string Name, string Phone)
        {
            CustomerList = new ObservableCollection<CustomerDTO>();
            if (ID == 0 && string.IsNullOrEmpty(Name) == true && string.IsNullOrEmpty(Phone) == true)
            {
                foreach (CustomerDTO item in CoreCustomerList)
                    CustomerList.Add(item);
            }
            else if (ID != 0)
            {
                foreach (CustomerDTO item in CoreCustomerList)
                {
                    if (ID.ToString().Contains(item.ID.ToString()))
                        CustomerList.Add(item);
                }
            }
            else if (string.IsNullOrEmpty(Name) == false && string.IsNullOrEmpty(Phone) == false)
            {
                foreach (CustomerDTO item in CoreCustomerList)
                {
                    if (item.Name.ToLower().Contains(Name.ToLower()) && item.Phone.Contains(Phone))
                        CustomerList.Add(item);
                }
            }
            else if (string.IsNullOrEmpty(Name) == true)
            {
                foreach (CustomerDTO item in CoreCustomerList)
                {
                    if (item.Phone.Contains(Phone))
                        CustomerList.Add(item);
                }
            }
            else if (string.IsNullOrEmpty(Phone) == true)
            {
                foreach (CustomerDTO item in CoreCustomerList)
                {
                    if (item.Name.ToLower().Contains(Name.ToLower()))
                        CustomerList.Add(item);
                }
            }
            else
            {
                CustomerList = CoreCustomerList;
            }
        }

    }
}
