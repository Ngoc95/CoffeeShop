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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private List<string> _TableStatusList;
        public List<string> TableStatusList
        {
            get { return _TableStatusList; }
            set { _TableStatusList = value; OnPropertyChanged(); }
        }

        private int _FilterGnereID = 0;
        public int FilterGnereID
        {
            get { return _FilterGnereID; }
            set { _FilterGnereID = value; OnPropertyChanged(); }
        }

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

        private int _EditingDay;
        public int EditingDay
        {
            set { _EditingDay = value; OnPropertyChanged(); }
            get { return _EditingDay; }
        }
        private int _EditingMonth;
        public int EditingMonth
        {
            get { return _EditingMonth; }
            set { _EditingMonth = value; OnPropertyChanged(); }
        }
        public int _EditingYear;
        public int EditingYear
        {
            get { return _EditingYear; }
            set { _EditingYear = value; OnPropertyChanged(); }
        }

        public int _EditingHour;
        public int EditingHour
        {
            get { return _EditingHour; }
            set { _EditingHour = value; OnPropertyChanged(); }
        }
        public int _EditingMinute;
        public int EditingMinute
        {
            get { return _EditingMinute; }
            set { _EditingMinute = value; OnPropertyChanged(); }
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
        private int _NewResDay;
        public int NewResDay
        {
            set { _NewResDay = value; OnPropertyChanged(); }
            get { return _NewResDay; }
        }
        private int _NewResMonth;
        public int NewResMonth
        {
            set { _NewResMonth = value; OnPropertyChanged(); }
            get { return _NewResMonth; }
        }
        private int _NewResYear;
        public int NewResYear
        {
            get { return _NewResYear; }
            set { _NewResYear = value; OnPropertyChanged(); }
        }

        private int _NewResHour;
        public int NewResHour
        {
            get { return _NewResHour; }
            set { _NewResHour = value; OnPropertyChanged(); }
        }
        private int _NewResMinute;
        public int NewResMinute
        {
            get { return _NewResMinute; }
            set { _NewResMinute = value; OnPropertyChanged(); }
        }

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
        public ICommand CustomerFilterCommand { get; set; }
        public ICommand CloseWDCommand { get; set; }
        public ICommand btnSaveReservationCommand { get; set; }
        public ICommand btnDeleteResWithoutSaveCommand { get; set; }

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

                TableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                
                if (TableStatusList == null)
                    TableStatusList = new List<string>() { "Còn trống", "Đang bận", "Đang sửa chữa" };

                if (ReservationList == null)
                    ReservationList = new ObservableCollection<ReservationDTO>(await ReservationService.Ins.GetAllReservation());
                //SelectedReservation = ReservationList[0];

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
                    NewReservation = new ReservationDTO() { RES_ID = ReservationService.Ins.GetNextResID(), NUM_OF_PEOPLE = 1 };
                }
            });

            FilterCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;
                string text;
                if (_CbbSelectedIndex == 0) text = null;
                else if (_CbbSelectedIndex == 1) text = "Còn trống";
                else if (_CbbSelectedIndex == 2) text = "Đang bận";
                else text = "Đang sửa chữa";

                if (FilterGnereID == 0 && text == null)
                {
                    TableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                    return;
                }
                TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(_FilterGnereID, text));

            });


            TableCheckCommand = new RelayCommand<TableDTO>((p) => { return true; }, async (p) =>
            {
                if (p.TB_STATUS == "Đang sửa chữa") return;
                p.TB_STATUS = p.TB_STATUS == "Còn trống" ? "Đang bận" : "Còn trống";
                await TableService.Ins.UpdateATable(p);
                string text;
                if (_CbbSelectedIndex == 0) text = null;
                else if (_CbbSelectedIndex == 1) text = "Còn trống";
                else if (_CbbSelectedIndex == 2) text = "Đang bận";
                else text = "Đang sửa chữa";
                TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(_FilterGnereID, text));
            });


            OpenResevationDetailCommand = new RelayCommand<ReservationDTO>((p) => { return true; }, (p) =>
            {
                EditingReservation = p;
                SelectedReservation = new ReservationDTO(p);
                SetEditingDateTime();
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
                if(SelectedReservation.IsEqual(EditingReservation))
                {
                    if(SelectedReservation.RES_STATUS == "Khách chưa nhận bàn")
                    {
                        SelectedReservation.RES_STATUS = "Khách đã nhận bàn";
                        (bool Res,string messs)  = await TableService.Ins.TableStatusIsAbleAndUpdate(SelectedReservation.TABLE_ID); 
                        if(Res)
                        {
                            await ReservationService.Ins.UpdateReservation(SelectedReservation);
                            ReservationList = new ObservableCollection<ReservationDTO>(await ReservationService.Ins.GetAllReservation());
                            p.Close();
                        }    
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, messs);
                            return;
                        }
                    }
                    else
                    {
                        SelectedReservation.RES_STATUS = "Khách chưa nhận bàn";
                        (bool Res, string messs) = await TableService.Ins.TableStatusUpdateChangeCheckin(SelectedReservation.TABLE_ID);
                        if (Res)
                        {
                            await ReservationService.Ins.UpdateReservation(SelectedReservation);
                            ReservationList = new ObservableCollection<ReservationDTO>(await ReservationService.Ins.GetAllReservation());
                            p.Close();
                        }
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, messs);
                            return;
                        }
                    }
                    string text;
                    if (_CbbSelectedIndex == 0) text = null;
                    else if (_CbbSelectedIndex == 1) text = "Còn trống";
                    else if (_CbbSelectedIndex == 2) text = "Đang bận";
                    else text = "Đang sửa chữa";
                    TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(_FilterGnereID, text));
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin thay đổi chưa được lưu");
                }
            });

            SaveReservationChangeCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                
                SetDateForEditingeservation();
                if(SelectedReservation.IsEqual(EditingReservation))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin không có thay đổi!");
                }
                else
                {
                    bool res = await ReservationService.Ins.UpdateReservation(EditingReservation);
                    if (res)
                    {
                        ReservationList = new ObservableCollection<ReservationDTO>(await ReservationService.Ins.GetAllReservation());
                        p.Close();
                    }
                }
            });

            OpenSearchCusWDCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                SearchCustomerIDstring = null; SearchCustomerName = null; SearchCustomerPhone = null;
                SearchingCustomer = new CustomerDTO(SelectedCustomer);
                CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCus()));
                CusForReservationWindow wd = new CusForReservationWindow();
                wd.ShowDialog();
            });

            btnSelectCustomerCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (SearchingCustomer != null)
                    SelectedCustomer = SearchingCustomer;
                SearchingCustomer = null;
                p.Close();
            });

            CustomerFilterCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                int SearchCustomerID;
                if (int.TryParse(SearchCustomerIDstring, out int temp))
                    SearchCustomerID = temp;
                else SearchCustomerID = 0;
                CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.SearchCus(SearchCustomerID, SearchCustomerName, SearchCustomerPhone));
            });

            CloseWDCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                SearchingCustomer = null;
                p.Close();
            });

            btnSaveReservationCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if(SetDateForNewReservation())
                {
                    NewReservation.CUS_ID = SelectedCustomer.ID;
                    bool res = await ReservationService.Ins.AddNewReservation(NewReservation);
                    if (res)
                    {
                        ReservationList = new ObservableCollection<ReservationDTO>(await ReservationService.Ins.GetAllReservation());
                        NewReservation = new ReservationDTO() { RES_ID = ReservationService.Ins.GetNextResID(), NUM_OF_PEOPLE = 1 };
                        SelectedCustomer = new CustomerDTO();
                        NewResHour = 0;
                        NewResMinute = 0;
                        NewResDay = 0;
                        NewResMonth = 0;
                        NewResYear = 0;
                    }
                }
            });
            btnDeleteResWithoutSaveCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                NewReservation = new ReservationDTO() { RES_ID = ReservationService.Ins.GetNextResID(), NUM_OF_PEOPLE = 1 };
                SelectedCustomer = new CustomerDTO();
                NewResHour = 0;
                NewResMinute = 0;
                NewResDay = 0;
                NewResMonth = 0;
                NewResYear = 0;
            });

        }

        private async void SetResCustomer()
        {
            if(SelectedReservation != null)
            {
                int ID = SelectedReservation.CUS_ID;
                ReservationCustome = await CustomerService.Ins.FindCustomerbyID(ID);
            }
        }
        private void SetEditingDateTime()
        {
            EditingDay = SelectedReservation.RES_DATETIME.Day;
            EditingMonth = SelectedReservation.RES_DATETIME.Month;
            EditingYear = SelectedReservation.RES_DATETIME.Year;
            EditingHour = SelectedReservation.RES_DATETIME.Hour;
            EditingMinute = SelectedReservation.RES_DATETIME.Minute;    
        }


        private bool SetDateForEditingeservation()
        {
            try
            {
                EditingReservation.RES_DATETIME = new DateTime(EditingYear, EditingMonth, EditingDay, EditingHour, EditingMinute, 0);
                return true;
            }
            catch 
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày giờ không hợp lệ");
                return false;
            }
        }

        private bool SetDateForNewReservation()
        {
            try
            {
                NewReservation.RES_DATETIME = new DateTime(NewResYear, NewResMonth, NewResDay, NewResHour, NewResMinute, 0);
                return true;
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày giờ không hợp lệ");
                return false;
            }
        }
    }
}
