using QuanLiCoffeeShop.MVVM.View.Admin;
using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Admin.ThongKeManagement.LichSuBan;
using QuanLiCoffeeShop.MVVM.View.Admin.ThongKeManagement.DoanhThu;
using QuanLiCoffeeShop.MVVM.View.Admin.ThongKeManagement.MonUaThich;
using QuanLiCoffeeShop.MVVM.ViewModel.Admin;
using QuanLiCoffeeShop.MVVM.View.Admin.ThongKeManagement;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using LiveCharts.Wpf;
using LiveCharts;
using System.Diagnostics;
using System.Xml.Linq;


namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    public partial class ThongKeViewModel : BaseViewModel
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        private DateTime _selectedDateFrom;
        public DateTime SelectedDateFrom
        {
            get { return _selectedDateFrom; }
            set 
            {
                _selectedDateFrom = value;
                OnPropertyChanged();
            }
        }
        private DateTime _selectedDateTo;
        public DateTime SelectedDateTo
        {
            get { return _selectedDateTo; }
            set 
            {
                _selectedDateTo = value; 
                OnPropertyChanged();
            }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand CloseWdCM { get; set; }
        public ICommand HistoryCM { get; set; }
        public ICommand RevenueCM { get; set; }
        public ICommand FavorCM { get; set; }
        public ICommand InfoBillCM { get; set; }
        public ICommand DeleteBillCM { get; set; }
        public ICommand DateChange { get; set; }

        public ThongKeViewModel()
        {
            SelectedDateFrom = SelectedDateFrom = DateTime.Today.AddDays(-2);
            SelectedDateTo = DateTime.Today;

            FirstLoadCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                try
                {
                    IsBusy = true; // Thêm trạng thái để báo cho giao diện biết ứng dụng đang xử lý
                    SelectedDateTo = DateTime.Today;
                    SelectedDateFrom = DateTime.Today.AddDays(-2);
                    SumBillTotal = 0;
                    var bills = await Task.Run(() => BillService.Ins.GetAllBill());
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        BillList = new ObservableCollection<BillDTO>(bills);
                        billList = new List<BillDTO>(BillList);
                        BillList = new ObservableCollection<BillDTO>(billList.FindAll(x => x.CREATE_AT >= SelectedDateFrom && x.CREATE_AT <= SelectedDateTo));
                        p.Content = new LichSuTable();
                    });
                }
                finally
                {
                    IsBusy = false;
                }
            });
            DateChange = new RelayCommand<object>((p) => {
                if (SelectedDateFrom > SelectedDateTo)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Khoảng thời gian không hợp lệ.");
                    return false;
                }
                return true;
            }, async (p) =>
            {
                try
                {
                    IsBusy = true;
                    await Task.Run(() =>
                    {
                        BillList = new ObservableCollection<BillDTO>(billList.FindAll(x => x.CREATE_AT >= SelectedDateFrom && x.CREATE_AT <= SelectedDateTo));
                    });

                    var revenueTasks = Enumerable.Range(0, (SelectedDateTo - SelectedDateFrom).Days + 1)
                                                 .Select(offset =>
                                                 {
                                                     DateTime currentDate = SelectedDateFrom.AddDays(offset);
                                                     return Task.Run(() => BillService.Ins.getBillByDate(currentDate));
                                                 });

                    var revenues = await Task.WhenAll(revenueTasks);

                    RevenueSeries = new SeriesCollection
                    {
                        new LineSeries
                        {
                            Title = "Doanh thu",
                            Values = new ChartValues<int>(revenues),
                        }
                    };
                    Labels = Enumerable.Range(0, revenues.Length)
                                       .Select(offset => SelectedDateFrom.AddDays(offset).ToString("dd/MM/yyyy"))
                                       .ToArray();
                    FavorList = new ObservableCollection<ProductDTO>(await Task.Run(() => ThongKeService.Ins.GetTop10SalerBetween(SelectedDateFrom, SelectedDateTo)));
                }
                finally
                {
                    IsBusy = false;
                }
            });
            HistoryCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new LichSuTable();

                BillList = new ObservableCollection<BillDTO>(billList.FindAll(x => x.CREATE_AT >= SelectedDateFrom && x.CREATE_AT <= SelectedDateTo));
            });

            CloseWdCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
            DeleteBillCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool sucess, string messageDelete) = await BillService.Ins.DeleteBill(SelectedItem);
                    if (sucess)
                    {
                        BillList.Remove(SelectedItem);
                        billList = new List<BillDTO>(BillList);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Xóa thành công");
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });
            #region DoanhThu
            RevenueCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                SumBillTotal = 0;
                p.Content = new DoanhThuTable();
                List<int> revenueValues = new List<int>();
                List<DateTime> dates = new List<DateTime>();
                DateTime currentDate = SelectedDateFrom;
                DateTime UpDate = SelectedDateTo.AddDays(1);
                while (currentDate <= UpDate)
                {
                    int revenue = await BillService.Ins.getBillByDate(currentDate);
                    revenueValues.Add(revenue);
                    SumBillTotal += revenue;
                    dates.Add(currentDate);
                    currentDate = currentDate.AddDays(1);
                }

                string[] dateStrings = dates.Select(date => date.ToString("dd/MM/yyyy")).ToArray();
                RevenueSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Doanh thu",
                        Values = new ChartValues<int>(revenueValues),
                    }
                };
                Labels = dateStrings;
                YFormatter = value =>
                {
                    return value.ToString("N");

                };
            });
            #endregion

            FavorCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                p.Content = new MonUaThichTable();
                FavorList = new ObservableCollection<ProductDTO>(await Task.Run(() => ThongKeService.Ins.GetTop10SalerBetween(SelectedDateFrom, SelectedDateTo)));
            });

            InfoBillCM = new RelayCommand<BillDTO>((p) => { return true; }, (p) =>
            {
                if (SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("SelectedItem null???");
                }
                else
                {
                    BillDTO a = SelectedItem;
                    CusName = SelectedItem.CUSTOMER.CUS_NAME;
                    EmpName = SelectedItem.EMPLOYEE.EMP_NAME;
                    BillDate = SelectedItem.CREATE_AT.ToString();
                    BillValue = SelectedItem.TOTAL_COST ?? 0;
                    BillDiscount = SelectedItem.DISCOUNT ?? 0;
                    ProductList = new ObservableCollection<Bill_InfoDTO>(SelectedItem.BillInfo);
                    ChiTietHoaDon wd = new ChiTietHoaDon();
                    wd.ShowDialog();
                }
            });
        }

    }
}
