using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
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

        #endregion

        #endregion

        #region MainCommand
        public ICommand FirstLoadCM {  get; set; }
        public ICommand FilterCommand {  get; set; }
        public ICommand TableCheckCommand {  get; set; }
        #endregion
        #region ReservationCommand
        public ICommand OpenResevationDetailCommand {  get; set; }

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
                SelectedReservation = ReservationList[0];
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
                SelectedReservation = p;
                SetResCustomer();
                SetEditingDate();
                if(SelectedReservation != null)
                {
                    ReservationInfor wd = new ReservationInfor();
                    wd.ShowDialog();
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra");
                }
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
        private void SetEditingDate()
        {
            _EditingDay = SelectedReservation.RES_DATE.Day;
            _EditingMonth = SelectedReservation.RES_DATE.Month;
            _EditingYear = SelectedReservation.RES_DATE.Year;
        }
    }
}
