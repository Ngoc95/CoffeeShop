using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.Staff.StaffOrderMenu;
using QuanLiCoffeeShop.MVVM.View.Staff.StaffTable;
using QuanLiCoffeeShop.MVVM.View.TableCard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    class StaffTableResViewModel : ObservableObject
    {
        #region Declare

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

        private List<string> _GenreTableNameList = new List<string>();
        public List<string> GenreTableNameList
        {
            get { return _GenreTableNameList; }
            set { _GenreTableNameList = value; OnPropertyChanged(); }
        }

        //dung de load du lieu len
        private ObservableCollection<TableDTO> _CoreTableList;
        public ObservableCollection<TableDTO> CoreTableList
        {
            get { return _TableList; }
            set
            {
                _TableList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TableDTO> _TableList;
        public ObservableCollection<TableDTO> TableList
        {
            get { return _CoreTableList; }
            set
            {
                _CoreTableList = value;
                OnPropertyChanged();
            }
        }

        private List<string> _TableStatusList;
        public List<string> TableStatusList
        {
            get { return _TableStatusList; }
            set { _TableStatusList = value; OnPropertyChanged(); }
        }

        private int FilterGnereID = 0;


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

        public CustomerDTO _ReservationCustomer;
        public CustomerDTO ReservationCustome
        {
            get { return _ReservationCustomer; }
            set { _ReservationCustomer = value; OnPropertyChanged(); }
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
        private bool _UseDateFilter;
        public bool UseDateFilter
        { get { return _UseDateFilter; } set { _UseDateFilter = value; OnPropertyChanged(); } }


        #endregion

        #endregion

        #region MainCommand
        public ICommand FirstLoadCM {  get; set; }
        public ICommand FilterCommand {  get; set; }
        public ICommand TableCheckCommand {  get; set; }
        #endregion
        #region ReservationCommand
        public ICommand OpenResevationDetailCommand {  get; set; }
        public ICommand btnCheckinCommand {  get; set; }
        public ICommand SaveReservationChangeCommand {  get; set; }
        public ICommand OpenSearchCusWDCommand {  get; set; }
        public ICommand btnSelectCustomerCommand { get; set; }
        public ICommand ReloadCustomerCommand { get; set; }
        public ICommand CustomerFilterCommand { get; set; }
        public ICommand btnSaveReservationCommand { get; set; }
        public ICommand btnDeleteResWithoutSaveCommand { get; set; }
        public ICommand SelectTableCommand { get; set; }
        public ICommand FilterReservationCommand { get; set; }
        public ICommand SetDateFilterCommand { get; set; }
        #endregion

        public StaffTableResViewModel()
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
                FilterGnereID = 0;
                CbbSelectedIndex = 0;

                if(CoreTableList == null)
                {
                    CoreTableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                    TableList = CoreTableList;
                }
                
                if (TableStatusList == null)
                    TableStatusList = new List<string>() { "Còn trống", "Đang bận", "Đang sửa chữa" };

                if (CoreReservationList == null)
                {
                    CoreReservationList = new ObservableCollection<ReservationDTO>(await ReservationService.Ins.GetAllReservation());
                    ReservationList = CoreReservationList;
                }


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

                FilterTableList();
                UseDateFilter = false;
            });

            FilterCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;
                FilterTableList();
            });

            TableCheckCommand = new RelayCommand<TableDTO>((p) => { return true; }, async (p) =>
            {
                if (p.TB_STATUS == "Đang sửa chữa") return;
                p.TB_STATUS = p.TB_STATUS == "Còn trống" ? "Đang bận" : "Còn trống";
                await TableService.Ins.UpdateATable(p);
                UpdateCoreTableList(p);
            });


            OpenResevationDetailCommand = new RelayCommand<ReservationDTO>((p) => { return true; }, (p) =>
            { 
                EditingReservation = new ReservationDTO(p);
                SelectedReservation = new ReservationDTO(EditingReservation);
                SetResCustomer();
                if(EditingReservation != null)
                {
                    ReservationInfor wd = new ReservationInfor();
                    wd.ShowDialog();
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra");
                }
            });
           
            btnCheckinCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if(SelectedReservation.IsEqual(EditingReservation))//bug
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
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Lưu thông tin thay đổi trước");
                }
            });

            SaveReservationChangeCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if(SelectedReservation.IsEqual(EditingReservation))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin không có thay đổi!");
                }
                else
                {
                    if(canSaveReservationChange(EditingReservation))
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
                }
            });

            OpenSearchCusWDCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                SearchCustomerIDstring = null; SearchCustomerName = null; SearchCustomerPhone = null;
                SearchingCustomer = new CustomerDTO(SelectedCustomer);
                CoreCustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCus()));
                CustomerList = CoreCustomerList;
                CusForReservationWindow wd = new CusForReservationWindow();
                wd.ShowDialog();
            });
            ReloadCustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CustomerList = new ObservableCollection<CustomerDTO>(CoreCustomerList);
                SearchCustomerPhone = "";
                SearchCustomerName = "";
                SearchCustomerIDstring = "";
            });


            btnSelectCustomerCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (SearchingCustomer != null)
                    SelectedCustomer = SearchingCustomer;
                SearchingCustomer = null;
                p.Close();
            });

            CustomerFilterCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                int SearchCustomerID;
                if (int.TryParse(SearchCustomerIDstring, out int temp))
                    SearchCustomerID = temp;
                else SearchCustomerID = 0;
                FilterCustomer(SearchCustomerID, SearchCustomerName, SearchCustomerPhone);
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

            btnDeleteResWithoutSaveCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                RecreateNewReservation();
                SelectedCustomer = new CustomerDTO();
            });

            SelectTableCommand = new RelayCommand<TableDTO>((p) => { return true; }, (p) =>
            {
                NewReservation.TABLE_ID = p.TB_ID;
                NewReservation = new ReservationDTO(NewReservation);
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

        }


        private async Task<bool> TableCanCheckIn(int tABLE_ID)
        {
            foreach (TableDTO table in CoreTableList)
            {
                if (tABLE_ID == table.TB_ID)
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

        private void UpdateCoreReservationList(ReservationDTO newReservation)
        {
            CoreReservationList.Add(newReservation);
            ReservationList = CoreReservationList;
        }

        private bool ResCanAddToDB(ReservationDTO reservation)
        {
            if (reservation.TABLE_ID == 0)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Chưa chọn bàn!!");
                return false;
            }
            else if (!SeatEnought(reservation.TABLE_ID, reservation.NUM_OF_PEOPLE))
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Bàn không phù hợp với số lượng khách!");
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


        private bool canSaveReservationChange(ReservationDTO SeleReservation)
        {
            if (SeleReservation.RES_DATE.Date < DateTime.Now.Date)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày đặt bàn không phù hợp");
                return false;
            }

            if (SeleReservation.RES_TIME.TimeOfDay < new TimeSpan(7, 0, 0) || SeleReservation.RES_TIME.TimeOfDay > new TimeSpan(20, 0, 0))
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Giờ đặt bàn từ 7h đến 20h");
                return false;
            }
            return true;
            
        }

        private void UpdateAIteminCoreReservationList(ReservationDTO selReservation)
        {
            for (int i = 0; i < CoreReservationList.Count; i++)
            {
                if(CoreReservationList[i].RES_ID == selReservation.RES_ID)
                {
                    CoreReservationList[i] = new ReservationDTO(selReservation);
                }
            }
            ReservationList = CoreReservationList;
        }

        private async Task<bool> CanUseTableNow(ReservationDTO SeReservation)
        {
            int i = 0;
            for (i = 0; i < TableList.Count(); i++)
            {
                if (TableList[i].TB_ID == SeReservation.TABLE_ID)
                    break;
            }
            if (TableList[i].TB_STATUS == "Còn trống")
            {
                TableList[i].TB_STATUS = "Đang bận";
                TableList[i] = new TableDTO(TableList[i]);
                await TableService.Ins.UpdateATable(TableList[i]);
                UpdateCoreTableList(TableList[i]);
                return true;
            }
            else
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Bàn đang bận hoặc đang sửa chữa, cân nhắc đổi bàn!");
                return false;
            }
        }

        private void UpdateCoreTableList(TableDTO table)
        {
            //da thay doi TableList
            for (int i = 0; i < CoreTableList.Count(); i++)
            {
                if (CoreTableList[i].TB_ID == table.TB_ID)
                {
                    //CoreTableList[i] = new TableDTO();
                    CoreTableList[i] = table;
                    FilterTableList();
                    return;
                }
            }
        }

        private void FilterTableList()
        {
            string Status;
            if (_CbbSelectedIndex == 0) Status = null;
            else if (_CbbSelectedIndex == 1) Status = "Còn trống";
            else if (_CbbSelectedIndex == 2) Status = "Đang bận";
            else Status = "Đang sửa chữa";
            if (FilterGnereID != 0 && CbbSelectedIndex != 0)
            {
                TableList = new ObservableCollection<TableDTO>();
                foreach(TableDTO item in CoreTableList)
                {
                    if (item.TB_STATUS.Equals(Status) && item.GT_ID == FilterGnereID)
                        TableList.Add(item);
                }
            }
            else if (FilterGnereID != 0 && CbbSelectedIndex == 0)
            {
                TableList = new ObservableCollection<TableDTO>();
                foreach (TableDTO item in CoreTableList)
                {
                    if (item.GT_ID == FilterGnereID)
                        TableList.Add(item);
                }
            }
            else if (CbbSelectedIndex != 0 && FilterGnereID == 0)
            {
                TableList = new ObservableCollection<TableDTO>();
                foreach (TableDTO item in CoreTableList)
                {
                    if (item.TB_STATUS.Equals(Status))
                        TableList.Add(item);
                }
            }
            else
            {
                TableList = new ObservableCollection<TableDTO>();
                foreach (TableDTO item in CoreTableList)
                {
                        TableList.Add(item);
                }
            }
        }

        private async void SetResCustomer()
        {
            if(SelectedReservation != null)
            {
                int ID = SelectedReservation.CUS_ID;
                ReservationCustome = await CustomerService.Ins.FindCustomerbyID(ID);
            }
        }

        private void FilterReservation(DateTime filterDateRes, int filterIndexRes)
        {
            ReservationList = new ObservableCollection<ReservationDTO>();
            string text;
            if (filterIndexRes == 0) text = null;
            else if (filterIndexRes == 1) text = "Khách đã nhận bàn";
            else text = "Khách chưa nhận bàn";
            if (UseDateFilter)
            {
                if (text == null)
                {
                    foreach (ReservationDTO reservation in CoreReservationList)
                    {
                        if (reservation.RES_DATE == filterDateRes)
                            ReservationList.Add(reservation);
                    }
                }
                else
                {
                    foreach (ReservationDTO reservation in CoreReservationList)
                    {
                        if (reservation.RES_DATE == filterDateRes && reservation.RES_STATUS == text)
                            ReservationList.Add(reservation);
                    }
                }
            }
            else if (text != null)
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
        private void AddCoreReservationList(ReservationDTO newReservation)
        {
            CoreReservationList.Add(newReservation);
            ReservationList = CoreReservationList;
        }

        private bool SeatEnought(int tABLE_ID, int numOfPP)
        {
            foreach (TableDTO item in CoreTableList)
            {
                if (item.TB_ID == tABLE_ID)
                {
                    int s = getGenreTable(item.GT_ID);
                    if (numOfPP > s)
                        return false;
                    else
                        return true;
                }
            }
            return false;
        }

        private int getGenreTable(int? gT_ID)
        {
            string s = "";
            foreach (GenreTableDTO item in GenreTableList)
            {
                if (item.GT_ID == gT_ID)
                {
                    int n = item.GT_NAME.Length;
                    s = new string(item.GT_NAME[n - 1], 1);
                    break;
                }
            }
            return int.Parse(s);
        }

    }
}
