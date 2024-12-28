using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.MVVM.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using QuanLiCoffeeShop.DTOs;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    internal class StaffHomeViewModel : ObservableObject
    {
        private ObservableCollection<List<string>> _GeneralResList;
        public ObservableCollection<List<string>> GeneralResList
        {
            get { return _GeneralResList; }
            set { _GeneralResList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<List<string>> _GeneralRequestList;
        public ObservableCollection<List<string>> GeneralRequestList
        {
            get { return _GeneralRequestList; }
            set { _GeneralRequestList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BillDTO> CoreBillEmpList;
        private ObservableCollection<BillDTO> _BillEmpList;
        public ObservableCollection<BillDTO> BillEmpList
        {
            get { return _BillEmpList; }
            set { _BillEmpList = value; OnPropertyChanged(); }
        }

        private DateTime _FilterBillDate;
        public DateTime FilterBillDate
        {
            get { return _FilterBillDate; }
            set { _FilterBillDate = value; OnPropertyChanged(); }
        }

        private bool _UseDateFilter;
        public bool UseDateFilter
        {
            get { return _UseDateFilter; }
            set { _UseDateFilter = value; OnPropertyChanged(); }
        }
        public ICommand FirstLoadcommand { get; set; }
        public ICommand SetDateFilterCommand { get; set; }
        public ICommand FilterReservationCommand { get; set; }
        public StaffHomeViewModel()
        {
            FirstLoadcommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                GenerateGeneralResList();
                GenerateGeneralRequestList();
                CoreBillEmpList = new ObservableCollection<BillDTO>(await BillService.Ins.GetBillByEmp(MainViewModel.currentEmp.EMP_ID));
                BillEmpList = new ObservableCollection<BillDTO>(CoreBillEmpList);
                UseDateFilter = false;
            });

            SetDateFilterCommand = new RelayCommand<DatePicker>((p) => { return true; }, (p) =>
            {
                if (p.Visibility == Visibility.Visible)
                {
                    p.Visibility = Visibility.Collapsed;
                }
                else
                {
                    p.Visibility = Visibility.Visible;
                    FilterBillDate = DateTime.Today;
                }
                FilterBill(FilterBillDate);
            });

            FilterReservationCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FilterBill(FilterBillDate);
            });
        }

        private void FilterBill(DateTime filterBillDate)
        {
            if(UseDateFilter)
            {
                BillEmpList = new ObservableCollection<BillDTO>();
                foreach(BillDTO item in CoreBillEmpList)
                {
                    if(item.CREATE_AT.Value.Date ==  filterBillDate.Date)
                        BillEmpList.Add(item);
                }
                BillEmpList = new ObservableCollection<BillDTO>(BillEmpList);
            }
            else
            {
                BillEmpList = CoreBillEmpList;
            }
        }

        private async void GenerateGeneralRequestList()
        {
            GeneralRequestList = new ObservableCollection<List<string>>();
            List<List<string>> temp = await RequestService.Ins.GetGenralRequestStaffList(MainViewModel.currentEmp.EMP_ID);
            if (temp == null) return;
            foreach(List<string> i in temp)
            {
                i.Add("pack://application:,,,/Images/Home/Reservation.png");
                GeneralRequestList.Add(i);
            }
        }

        private async void GenerateGeneralResList()
        {
            GeneralResList = new ObservableCollection<List<string>>();
            List<List<string>> temp = await ReservationService.Ins.GetGenralResList();
            if (temp == null) return;

            foreach (var item in temp)
            {
                item.Add("pack://application:,,,/Images/Home/Reservation.png");
                GeneralResList.Add(item);
            }
        }

    }
}
