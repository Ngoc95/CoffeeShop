using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.TableManagament;
using QuanLiCoffeeShop.MVVM.View.Admin.TableManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.Staff.StaffTable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class TableViewModel : ObservableObject
    {
        #region TableTab1
        private int _CbbSelectedIndex = 0;
        public int CbbSelectedIndex
        {
            get { return _CbbSelectedIndex; }
            set 
            {
                if (_CbbSelectedIndex != value)
                {
                    _CbbSelectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _UseDateFilter; 
        public bool UseDateFilter 
        { get { return _UseDateFilter; }  set { _UseDateFilter = value; OnPropertyChanged(); } }

        private int _FilterGnereID = 0;
        public int FilterGnereID
        {
            get { return _FilterGnereID; }
            set { _FilterGnereID = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GenreTableDTO> _GenreTableList;
        public ObservableCollection<GenreTableDTO> GenreTableList 
        {
            get { return _GenreTableList; }
            set
            {
                _GenreTableList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TableDTO> CoreTableList;
        private ObservableCollection<TableDTO> _TableList;
        public ObservableCollection<TableDTO> TableList
        {
            get { return _TableList; }
            set
            {
                _TableList = value;
                OnPropertyChanged();
            }
        }

        private TableDTO _SelectedTable;
        public TableDTO SelectedTable
        {
            get { return _SelectedTable; }
            set { _SelectedTable = value; OnPropertyChanged(); }
        }

        private List<string> _GenreTableNameList = new List<string>();
        public List<string> GenreTableNameList
        {
            get { return _GenreTableNameList; }
            set { _GenreTableNameList = value; OnPropertyChanged(); }
        }

        private string _SelectedTableGenreName;
        public string SelectedTableGenreName
        {
            get { return _SelectedTableGenreName; }
            set { _SelectedTableGenreName = value; OnPropertyChanged(); }
        }

        private List<string> _TableStatusList;
        public List<string> TableStatusList
        {
            get { return _TableStatusList; }
            set { _TableStatusList = value; OnPropertyChanged(); }
        }

        private int _IDOfNextTable = 0;
        public int IDOfNextTable
        {
            get { return _IDOfNextTable; }
            set
            {
                if (_IDOfNextTable != value)
                {
                    _IDOfNextTable = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region Reservation
        private ObservableCollection<ReservationDTO> CoreReservationList;
        private ObservableCollection<ReservationDTO> _ReservationList;
        public ObservableCollection<ReservationDTO> ReservationList
        {
            get { return _ReservationList; }
            set { _ReservationList = value; OnPropertyChanged(); }
        }
        private ReservationDTO _SelectedReserVation;
        public ReservationDTO SelectedReservation
        {
            get { return _SelectedReserVation; }
            set { _SelectedReserVation = value; OnPropertyChanged(); }
        }
        private ReservationDTO _EditingReservetion;
        public ReservationDTO EditingReservation
        {
            get { return _EditingReservetion; }
            set { _EditingReservetion = value; OnPropertyChanged(); }
        }
        private CustomerDTO _ReservationCustomer;
        public CustomerDTO ReservationCustome
        {
            get { return _ReservationCustomer; }
            set { _ReservationCustomer = value; OnPropertyChanged(); }
        }

        private int _FilterIndexReservation;
        public int FilterIndexReservation
        {
            get { return _FilterIndexReservation; }
            set { _FilterIndexReservation = value; OnPropertyChanged(); }
        }
        private DateTime _FilterDateReservation = DateTime.Now;
        public DateTime FilterDateReservation
        {
            get { return _FilterDateReservation; }
            set { _FilterDateReservation = value; OnPropertyChanged(); }
        }


        private CustomerDTO _SelectedCustomer;
        public CustomerDTO SelectedCustomer
        {
            set { _SelectedCustomer = value; OnPropertyChanged(); }
            get { return _SelectedCustomer; }
        }
        private CustomerDTO _SearchingCustomer;
        public CustomerDTO SearchingCustomer
        {
            set { _SearchingCustomer = value; OnPropertyChanged(); }
            get { return _SearchingCustomer; }
        }
        private string _SearchCustomerIDstring;
        public string SearchCustomerIDstring
        {
            get { return _SearchCustomerIDstring; }
            set { _SearchCustomerIDstring = value; OnPropertyChanged(); }
        }
        private string _SearchCustomerName;
        public string SearchCustomerName
        {
            get { return _SearchCustomerName; }
            set { _SearchCustomerName = value; OnPropertyChanged(); }
        }
        private string _SearchCustomerPhone;
        public string SearchCustomerPhone
        {
            get { return _SearchCustomerPhone; }
            set { _SearchCustomerPhone = value; OnPropertyChanged(); }
        }

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

        private ReservationDTO _NewReservation;
        public ReservationDTO NewReservation
        {
            set { _NewReservation = value; OnPropertyChanged(); }
            get { return _NewReservation; }
        }

        #endregion



        #region Command
        //For Table
        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenEditTableWDCommand { get; set; }
        public ICommand OpenDeleteTableWDCommand { get; set; }
        public ICommand OpenAddWDCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand btnCloseTableCommand { get; set; }
        public ICommand btnEditTableCommand { get; set; }
        public ICommand btnAddTableCommand { get; set; }

        //For Reservation
        public ICommand OpenResevationDetailCommand { get; set; }
        public ICommand btnCheckinAdminCommand { get; set; }
        public ICommand SaveReservationChangeAdminCommand { get; set; }
        public ICommand FilterReservationCommand { get; set; }
        public ICommand SetDateFilterCommand { get; set; }
        public ICommand OpenSearchCusWDCommand { get; set; }
        public ICommand CustomerFilterCommand { get; set; }
        public ICommand btnSelectCustomerCommand { get; set; }
        public ICommand btnSaveReservationCommand { get; set; }
        public ICommand btnDeleteReservationCommand { get; set; }
        public ICommand btnDeleteResWithoutSaveCommand { get; set; }

        #endregion

        public TableViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                if (GenreTableList == null)
                {
                    GenreTableList = new ObservableCollection<GenreTableDTO>(await GenreTableService.Ins.GetAllGenre());
                    foreach (var item in GenreTableList)
                    {
                        GenreTableNameList.Add(item.GT_NAME);
                    }
                    GenreTableList.Insert(0, new GenreTableDTO() { GT_ID = 0, GT_NAME = "Tất cả" });
                }

                if (CoreTableList == null)
                {
                    CoreTableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                    TableList = new ObservableCollection<TableDTO>(CoreTableList);
                }

                if (TableStatusList == null)
                    TableStatusList = new List<string>() { "Còn trống", "Đang bận", "Đang sửa chữa" };
                if (CoreReservationList == null)
                {
                    CoreReservationList = new ObservableCollection<ReservationDTO>(await ReservationService.Ins.GetAllReservation());
                    ReservationList = new ObservableCollection<ReservationDTO>(CoreReservationList);
                }

                FilterGnereID = 0;
                CbbSelectedIndex = 0;
                UseDateFilter = false;
                FilterTableList(FilterGnereID, CbbSelectedIndex);

                if (SelectedCustomer == null)
                {
                    SelectedCustomer = new CustomerDTO()
                    {
                        ID = 0,
                    };
                }
                else
                {
                    SelectedCustomer.ID = 0;
                }
                if (NewReservation == null)
                {
                    RecreateNewReservation();
                }
            });



            OpenEditTableWDCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                _SelectedTable = new TableDTO(p.DataContext as TableDTO);
                SelectedTableGenreName = GetGenreSelectedTableGenreName();
                Window wd = new EditTableWindow();
                wd.ShowDialog();
            });

            OpenAddWDCommand = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                IDOfNextTable = await TableService.Ins.NumOfTable() + 1;
                SelectedTable = new TableDTO();
                SelectedTableGenreName = null;
                Window wd = new AddTableWindow();
                wd.ShowDialog();
            });

            OpenDeleteTableWDCommand = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                SelectedTable = p.DataContext as TableDTO;
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool IsDeleted, string messageDelete) = await TableService.Ins.DeleteTableList(SelectedTable.TB_ID);
                    if (IsDeleted)
                    {
                        DeleteATBFromCoreTableList(SelectedTable);
                        FilterTableList(FilterGnereID, CbbSelectedIndex);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });

            FilterCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;

                FilterTableList(FilterGnereID, CbbSelectedIndex);
            });

            btnCloseTableCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
                SelectedTable = null; //co the bug
            });

            btnEditTableCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                SelectedTable.GT_ID = GetGenreSelectedTableGenreID();
                bool IsAdded = await TableService.Ins.UpdateATable(SelectedTable);
                if (IsAdded)
                {
                    p.Close();
                    UpdateATableFromCoreTableList(SelectedTable);
                    FilterTableList(FilterGnereID, CbbSelectedIndex);
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Chỉnh sửa thành công");
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thất bại khi try cập Database");
                }

            });

            btnAddTableCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                SelectedTable.GT_ID = GetGenreSelectedTableGenreID();
                (bool IsAdded, string messageAdd) = await TableService.Ins.AddTableList(SelectedTable);
                if (IsAdded)
                {
                    p.Close();
                    AddTableToCoreList(SelectedTable);
                    FilterTableList(FilterGnereID, CbbSelectedIndex);
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }

            });

            OpenResevationDetailCommand = new RelayCommand<ReservationDTO>((p) => { return true; }, (p) =>
            {
                EditingReservation = new ReservationDTO(p);
                SelectedReservation = p;
                SetResCustomer();
                if (EditingReservation != null)
                {
                    ResInforAdminWindow wd = new ResInforAdminWindow();
                    wd.ShowDialog();
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra");
                }

            });

            btnCheckinAdminCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (SelectedReservation.IsEqual(EditingReservation))
                {
                    if (SelectedReservation.RES_STATUS == "Khách chưa nhận bàn")
                    {
                        bool temp = await TableCanCheckIn(SelectedReservation.TABLE_ID);
                        if (temp)
                        {
                            SelectedReservation.RES_STATUS = "Khách đã nhận bàn";
                            await ReservationService.Ins.UpdateReservation(SelectedReservation);
                            UpdateCoreReservationList(SelectedReservation);
                            FilterReservation(FilterDateReservation, FilterIndexReservation);
                            p.Close();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        SelectedReservation.RES_STATUS = "Khách chưa nhận bàn";
                        await ReservationService.Ins.UpdateReservation(SelectedReservation);
                        UpdateCoreReservationList(SelectedReservation);
                        FilterReservation(FilterDateReservation, FilterIndexReservation);
                        p.Close();
                    }
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin thay đổi chưa được lưu");
                }

            });

            SaveReservationChangeAdminCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (SelectedReservation.IsEqual(EditingReservation))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin không có thay đổi!");
                }
                else if(CanChangeReservationDetail(EditingReservation))
                {
                    bool res = await ReservationService.Ins.UpdateReservation(EditingReservation);
                    if (res)
                    {
                        UpdateCoreReservationList(EditingReservation);
                        FilterReservation(FilterDateReservation, FilterIndexReservation);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Chỉnh sửa thành công");
                        p.Close();
                    }
                }
            });

            SetDateFilterCommand = new RelayCommand<DatePicker>((p) => { return true; }, (p) =>
            {
                if (p.Visibility == Visibility.Visible)
                {
                    p.Visibility = Visibility.Hidden;
                }
                else
                {
                    p.Visibility = Visibility.Visible;
                }
                FilterReservation(FilterDateReservation, FilterIndexReservation);
            });

            FilterReservationCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FilterReservation(FilterDateReservation, FilterIndexReservation);
            });

            OpenSearchCusWDCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                SearchCustomerIDstring = null; SearchCustomerName = null; SearchCustomerPhone = null;
                SearchingCustomer = new CustomerDTO(SelectedCustomer);
                if (CoreCustomerList == null)
                {
                    CoreCustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCus()));
                    CustomerList = new ObservableCollection<CustomerDTO>(CoreCustomerList);
                }
                CusForAdminResWindow wd = new CusForAdminResWindow();
                wd.ShowDialog();
            });

            CustomerFilterCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                int SearchCustomerID;
                if (int.TryParse(SearchCustomerIDstring, out int temp))
                    SearchCustomerID = temp;
                else SearchCustomerID = 0;
                FilterCustomer(SearchCustomerID, SearchCustomerName, SearchCustomerPhone);
            });

            btnSelectCustomerCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (SearchingCustomer != null)
                    SelectedCustomer = SearchingCustomer;
                SearchingCustomer = null;
                p.Close();
            });

            btnSaveReservationCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                NewReservation.CUS_ID = SelectedCustomer.ID;
                if (ResCanAddToDB(NewReservation))
                {
                    bool res = await ReservationService.Ins.AddNewReservation(NewReservation);
                    if (res)
                    {
                        NewReservation.RES_STATUS = "Khách chưa nhận bàn";
                        NewReservation.CREATE_AT = DateTime.Now;
                        AddCoreReservationList(NewReservation);
                        RecreateNewReservation();
                        SelectedCustomer = new CustomerDTO();
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Thêm lịch thành công");
                    }
                    else
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Thất bại khi truy cập Database");

                }
            });

            btnDeleteReservationCommand = new RelayCommand<ReservationDTO>((p) => { return true; }, async (p) =>
            {
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    bool temp = await ReservationService.Ins.DeleteAReservation(p);
                    DeleteAResFromCoreList(p);
                }
            });
            btnDeleteResWithoutSaveCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                RecreateNewReservation();
                SelectedCustomer = new CustomerDTO();
            });
        }

        private void UpdateCoreReservationList(ReservationDTO selected_editingReservation)
        {
            foreach(ReservationDTO item in CoreReservationList)
            {
                if(item.RES_ID == selected_editingReservation.RES_ID)
                { 
                    item.RES_DATE = selected_editingReservation.RES_DATE;
                    item.RES_STATUS = selected_editingReservation.RES_STATUS;
                    item.RES_TIME = selected_editingReservation.RES_TIME;
                    item.TABLE_ID = selected_editingReservation.TABLE_ID;
                    item.SPECIAL_REQUEST = selected_editingReservation.SPECIAL_REQUEST;
                    item.NUM_OF_PEOPLE = selected_editingReservation.NUM_OF_PEOPLE;
                    ReservationList = new ObservableCollection<ReservationDTO>(CoreReservationList);
                }
            }
        }

        private async Task<bool> TableCanCheckIn(int tABLE_ID)
        {
            foreach(TableDTO table in  CoreTableList)
            {
                if(tABLE_ID == table.TB_ID)
                {
                    if (table.TB_STATUS == "Còn trống")
                    {
                        table.TB_STATUS = "Đang bận";
                        await TableService.Ins.UpdateATable(table);
                        return true;
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Bàn đang bận hoặc đang sửa chữa, cân nhắc đổi bàn");
                        return false;
                    }
                }
            }
            MessageBoxCustom.Show(MessageBoxCustom.Error, "Mã bàn không tồn tại, có thể đã bị xóa, cân nhắc đổi bàn");
            return false;
        }

        private void AddTableToCoreList(TableDTO selectedTable)
        {
            CoreTableList.Add(selectedTable);
            TableList = new ObservableCollection<TableDTO>(CoreTableList);
        }

        private void UpdateATableFromCoreTableList(TableDTO selectedTable)
        {
            foreach(TableDTO table in CoreTableList)
            {
                if(table.TB_ID == selectedTable.TB_ID)
                {
                    table.TB_STATUS = selectedTable.TB_STATUS;
                    table.GT_ID = selectedTable.GT_ID;
                    TableList = new ObservableCollection<TableDTO>(CoreTableList);
                    return;
                }
            }
        }

        private bool CanChangeReservationDetail(ReservationDTO editingReservation)
        {
            if(!TableExists(editingReservation.TABLE_ID))
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Mã bàn không tồn tại");
                return false;
            }
            if (editingReservation.RES_TIME.TimeOfDay < new TimeSpan(7, 0, 0) || editingReservation.RES_TIME.TimeOfDay > new TimeSpan(20, 0, 0))
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Giờ đặt bàn từ 7h đến 20h");
                return false;
            }

            foreach (ReservationDTO item in CoreReservationList)
            {
                if (item.TABLE_ID == editingReservation.TABLE_ID
                    && item.RES_TIME.TimeOfDay == editingReservation.RES_TIME.TimeOfDay
                    && item.RES_DATE.Date == editingReservation.RES_DATE.Date)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Đã tồn tại lịch đặt bàn này vào cùng thời điểm");
                    return false;
                }
            }
            return true;
        }

        private void DeleteATBFromCoreTableList(TableDTO selected_Table)
        {
            for (int i = 0; i < CoreTableList.Count; i++)
            {
                if (CoreTableList[i].TB_ID == selected_Table.TB_ID)
                {
                    CoreTableList.RemoveAt(i);
                    TableList = new ObservableCollection<TableDTO>(CoreTableList);
                    return;
                }
            }
        }

        private void DeleteAResFromCoreList(ReservationDTO p)
        {
            for (int i = 0; i < CoreReservationList.Count(); i++)
            {
                if (CoreReservationList[i].RES_ID == p.RES_ID)
                {
                    CoreReservationList.RemoveAt(i);
                    ReservationList = new ObservableCollection<ReservationDTO>(CoreReservationList);
                    return;
                }
            }
        }

        private void RecreateNewReservation()
        {
            int temp = ReservationService.Ins.GetNextResID();
            NewReservation = new ReservationDTO()
            {
                RES_ID = temp,
                NUM_OF_PEOPLE = 1,
                RES_DATE = DateTime.Now,
                RES_TIME = DateTime.Now,
            };
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
                    if (item.Name.Contains(Name) && item.Phone.Contains(Phone))
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
                    if (item.Name.Contains(Name))
                        CustomerList.Add(item);
                }
            }
            else
            {
                CustomerList = CoreCustomerList;
            }
        }

        private void FilterReservation(DateTime filterDateRes, int filterIndexRes)
        {
            ReservationList = new ObservableCollection<ReservationDTO>();
            string text;
            if (filterIndexRes == 0) text = null;
            else if (filterIndexRes == 1) text = "Khách đã nhận bàn";
            else text = "Khách chưa nhận bàn";
            if(UseDateFilter)
            {
                if(text == null)
                {
                    foreach(ReservationDTO reservation in CoreReservationList)
                    {
                        if(reservation.RES_DATE ==  filterDateRes)
                            ReservationList.Add(reservation);
                    }
                }else
                {
                    foreach (ReservationDTO reservation in CoreReservationList)
                    {
                        if (reservation.RES_DATE == filterDateRes && reservation.RES_STATUS == text)
                            ReservationList.Add(reservation);
                    }
                }
            }
            else if(text != null)
            {
                foreach (ReservationDTO reservation in CoreReservationList)
                {
                    if (reservation.RES_STATUS == text)
                        ReservationList.Add(reservation);
                }
            }
            else
            {
                ReservationList = CoreReservationList;

            }
        }

        private void FilterTableList(int FilterSelectGenreID, int cbbIndex)
        {
            string text;
            if (cbbIndex == 0) text = null;
            else if (cbbIndex == 1) text = "Còn trống";
            else if (cbbIndex == 2) text = "Đang bận";
            else text = "Đang sửa chữa";
            TableList = new ObservableCollection<TableDTO>();
            if (FilterSelectGenreID != 0 && text != null)
            {
                foreach (TableDTO table in CoreTableList)
                {
                    if(table.TB_STATUS == text && table.GT_ID == FilterSelectGenreID)
                        TableList.Add(table);
                }
            }
            else if (FilterSelectGenreID != 0)
            {
                foreach (TableDTO table in CoreTableList)
                {
                    if (table.GT_ID == FilterSelectGenreID)
                        TableList.Add(table);
                }
            }
            else if (text != null)
            {
                foreach (TableDTO table in CoreTableList)
                {
                    if (table.TB_STATUS == text)
                        TableList.Add(table);
                }
            }
            else
            {
                TableList = CoreTableList;
            }
        }

        string GetGenreSelectedTableGenreName()
        {
            foreach(var item in GenreTableList)
            {
                if(item.GT_ID == SelectedTable.GT_ID)
                {
                    return item.GT_NAME;
                }
            }
            return null;
        }

        int GetGenreSelectedTableGenreID()
        {
            foreach(var item in GenreTableList)
            {
                if(SelectedTableGenreName ==  item.GT_NAME)
                    return item.GT_ID;
            }
            return 0;
        }

        private async void SetResCustomer()
        {
            if (SelectedReservation != null)
            {
                int ID = SelectedReservation.CUS_ID;
                ReservationCustome = await CustomerService.Ins.FindCustomerbyID(ID);
            }
        }

        private bool ResCanAddToDB(ReservationDTO reservation)
        {
            if (reservation.TABLE_ID == 0)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Chưa chọn bàn!!");
                return false;
            } else if(!TableExists(reservation.TABLE_ID))
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Mã bàn không tồn tại");
                return false;
            }

            if (reservation.CUS_ID == 0)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Chưa chọn khách hàng đặt bàn!!");
                return false;
            }
            if (reservation.RES_DATE.Date < DateTime.Now.Date)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày đặt bàn không phù hợp");
                return false;
            }
            if (reservation.RES_DATE.Date == DateTime.Now.Date && reservation.RES_TIME.TimeOfDay < DateTime.Now.TimeOfDay)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Giờ đặt bàn không phù hợp");
                return false;
            }

            if (reservation.RES_TIME.TimeOfDay < new TimeSpan(7, 0, 0) || reservation.RES_TIME.TimeOfDay > new TimeSpan(20, 0, 0))
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Giờ đặt bàn từ 7h đến 20h");
                return false;
            }

            foreach (ReservationDTO item in ReservationList)
            {
                if (item.TABLE_ID == reservation.TABLE_ID
                    && item.RES_TIME.TimeOfDay == reservation.RES_TIME.TimeOfDay
                    && item.RES_DATE.Date == reservation.RES_DATE.Date)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Đã tồn tại lịch đặt bàn này vào cùng thời điểm");
                    return false;
                }
            }
            return true;
        }

        private bool TableExists(int tABLE_ID)
        {
            foreach (TableDTO item in CoreTableList)
            {
                if(item.TB_ID == tABLE_ID)
                    return true;
            }
            return false;
        }

        private void AddCoreReservationList(ReservationDTO newReservation)
        {
            CoreReservationList.Add(newReservation);
            ReservationList = CoreReservationList;
        }

    }
}
