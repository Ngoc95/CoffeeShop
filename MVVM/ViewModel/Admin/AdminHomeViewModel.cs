using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using LiveCharts;
using LiveCharts.Wpf;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.MenuManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using LiveCharts.Defaults;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class AdminHomeViewModel : ObservableObject
    {
        private ObservableCollection<Tuple<string, string>> _NoticeList;
        public ObservableCollection<Tuple<string, string>> NoticeList
        {
            get { return _NoticeList; }
            set { _NoticeList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<List<string>> _GeneralList;
        public ObservableCollection<List<string>> GeneralList
        {
            get { return _GeneralList; }
            set { _GeneralList = value; OnPropertyChanged(); }
        }
        private SeriesCollection _TopSellerSeries;
        public SeriesCollection TopSellerSeries
        {
            set { _TopSellerSeries = value; OnPropertyChanged(); }
            get { return _TopSellerSeries; }
        }
        private SeriesCollection _EmployeeSeries;
        public SeriesCollection EmployeeSeries
        {
            set { _EmployeeSeries = value; OnPropertyChanged(); }
            get { return _EmployeeSeries; }
        }

        private List<Tuple<string, int>> TopSeller;
        private List<Tuple<string, int>> Employee;


        #region Command
        public ICommand FirstLoadcommand { get; set; }
        #endregion
        public AdminHomeViewModel()
        {
            FirstLoadcommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GenerateNoticeList();
                GenerateGeneralList();
                SetTopSellerChart();
                SetEmployeeChart();
            });
        }

        private void SetEmployeeChart()
        {
            Employee = EmployeeService.Ins.GetEmployee();
            EmployeeSeries = new SeriesCollection();
            if (TopSeller == null)
                return;
            foreach (var item in Employee)
            {
                EmployeeSeries.Add(new PieSeries()
                {
                    Title = item.Item1,
                    DataLabels = true,
                    FontSize = 16,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(item.Item2) }
                });
            }
        }

        private void SetTopSellerChart()
        {
            TopSeller = Bill_InfoService.Ins.GetTopSeller();
            TopSellerSeries = new SeriesCollection();
            if (TopSeller == null)
                return;
            foreach (var item in TopSeller)
            {
                TopSellerSeries.Add(new PieSeries()
                {
                    Title = item.Item1,
                    DataLabels = true,
                    FontSize = 16,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(item.Item2) }
                });
            }
        }

        private async void GenerateGeneralList()
        {
            GeneralList = new ObservableCollection<List<string>>();
            List<string> item;
            item = await GetProductGeneral();
            GeneralList.Add(item);
            item = await GetEmpGeneral();
            GeneralList.Add(item);
            item = await GetCusGeneral();
            GeneralList.Add(item);
            item = await GetTableGeneral();
            GeneralList.Add(item);
            item = await GetResGeneral();
            GeneralList.Add(item);
            item = await GetSupplierGeneral();
            GeneralList.Add(item);
        }

        private async Task<List<string>> GetSupplierGeneral()
        {
            string t = await SupplierService.Ins.GetSupplierGeneral();
            return new List<string>() { "Nhà cung cấp", t, "pack://application:,,,/Images/Home/supplier.png" };
        }

        private async Task<List<string>> GetResGeneral()
        {
            string t = await ReservationService.Ins.ReservationGenaral();
            return new List<string>() { "Lịch đặt bàn", t, "pack://application:,,,/Images/Home/Reservation.png" };
        }

        private async Task<List<string>> GetTableGeneral()
        {
            string t = await TableService.Ins.TableGenaral();
            return new List<string>() { "Bàn", t, "pack://application:,,,/Images/Home/table.png" };
        }

        private async Task<List<string>> GetCusGeneral()
        {
            string t = await CustomerService.Ins.NumOfCus();
            return new List<string>() { "Khách hàng", t, "pack://application:,,,/Images/Home/customer.png" };
        }

        private async Task<List<string>> GetEmpGeneral()
        {
            int t = await ProductService.Ins.NumOfProduct();
            return new List<string>() { "Đồ uống", t.ToString(), "pack://application:,,,/Images/Home/product.png" };
        }

        private async Task<List<string>> GetProductGeneral()
        {
            string t = await EmployeeService.Ins.NumOfEmployee();
            return new List<string>() {"Nhân viên", t, "pack://application:,,,/Images/Home/employee.png"};
        }

        private async void GenerateNoticeList()
        {
            NoticeList = new ObservableCollection<Tuple<string, string>>();
            Tuple<string, string> item;
            item = await GetTableStatusNotice();
            NoticeList.Add(item);
            item = await GetReservationStatus();
            NoticeList.Add(item); 
            item = await GetBillToday();
            NoticeList.Add(item);
            item = await GetEmpRequest();
            NoticeList.Add(item);
            item = await GetWorkToday();
            NoticeList.Add(item);
            item = await GetError();
            NoticeList.Add(item);
        }

        private async Task<Tuple<string, string>> GetError()
        {
            int a;
            a = await ErrorService.Ins.NumOfUnSolve(); 
            return new Tuple<string, string>("Sự cố", $"Có {a} sự cố chưa khắc phục.");
        }

        private async Task<Tuple<string, string>> GetWorkToday()
        {
            List<string> a;
            string b = "";
            a = await WorkshiftService.Ins.EmployeeWorkToday();
            foreach(string x in a)
            {
                b += x + "\n";
            }
            b = b.Substring(0, b.Length - 1);   
            return new Tuple<string, string>("Nhân viên làm việc", b);
        }

        private async Task<Tuple<string, string>> GetEmpRequest()
        {
            int a, b;
            (a, b) = await RequestService.Ins.RequestStatus();
            return new Tuple<string, string>("Yêu cầu chờ duyệt", $"Xin nghỉ: {a}\nĐổi ca: {b}");
        }

        private async Task<Tuple<string, string>> GetBillToday()
        {
            int a, b, c;
            (a, b) = await BillService.Ins.BillToday();
            c = a + b;
            return new Tuple<string, string>("Số hóa đơn", $"Từ khách vãng lai: {a}\nTừ khách đã đăng kí: {b}\nTổng số: {c}");
        }

        private async Task<Tuple<string, string>> GetReservationStatus()
        {
            int a, b;
            (a, b) = await ReservationService.Ins.GetResStatusToday();
            return new Tuple<string, string>("Lịch đặt bàn", $"Đã nhận bàn {a}\nChưa nhận bàn {b}");
        }

        private async Task<Tuple<string, string>> GetTableStatusNotice()
        {
            int a, b, c;
            (a, b, c)= await TableService.Ins.TableStatus();
            return new Tuple<string, string>("Trạng thái bàn", $"Còn trống {a}\nĐang bận {b}\nĐang sửa chữa {c}");
        }
    }
}
