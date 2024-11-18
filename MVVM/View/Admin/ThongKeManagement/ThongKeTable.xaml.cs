using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiCoffeeShop.MVVM.View.Admin.ThongKeManagement
{
    /// <summary>
    /// Interaction logic for ThongKeTable.xaml
    /// </summary>
    public partial class ThongKeTable : UserControl
    {
        public ThongKeTable()
        {
            InitializeComponent();
            LoadDataNgay();
            LoadDataThang();
        }
        private bool isThongKeClicked = false;
        private bool isBieuDoClicked = false;
        private bool isSoLuongChecked = false;

        private void ThongKeDay_Button_Click(object sender, RoutedEventArgs e)
        {
            btnThongKeNgay.Background = new SolidColorBrush(Color.FromArgb(255, 240, 153, 125));
            btnThongKeThang.Background = new SolidColorBrush(Colors.White);
            isThongKeClicked = true;
            CheckAndShowOverlayChart();
        }

        private void ThongKeThang_Button_Click(object sender, RoutedEventArgs e)
        {
            btnThongKeThang.Background = new SolidColorBrush(Color.FromArgb(255, 240, 153, 125));
            btnThongKeNgay.Background = new SolidColorBrush(Colors.White);
            OverlayGridDSTheoNgay.Visibility = Visibility.Collapsed;
            OverlayGridDSTheoThang.Visibility = Visibility.Visible;
            isThongKeClicked = true;
            CheckAndShowOverlayChart();
        }

        private void btnDanhSach_Click(object sender, RoutedEventArgs e)
        {
            OverlayGridDSTheoNgay.Visibility = Visibility.Visible;
        }



        private void btnXuatExcelOverlay_Click(object sender, RoutedEventArgs e)
        {
            // Code xử lý xuất Excel
            MessageBox.Show("Xuất dữ liệu ra Excel!");
            OverlayGridDSTheoNgay.Visibility = Visibility.Collapsed;
        }

        private void LoadDataNgay()
        {
            var data = new List<ItemThongKeNgay>
            {
                new ItemThongKeNgay { STT = 1, NgayThongKe = "01/2024", SoLuong = 240 },
                new ItemThongKeNgay { STT = 2, NgayThongKe = "02/2024", SoLuong = 255 },
                new ItemThongKeNgay { STT = 3, NgayThongKe = "03/2024", SoLuong = 180 },
                new ItemThongKeNgay { STT = 4, NgayThongKe = "04/2024", SoLuong = 233 },
                new ItemThongKeNgay { STT = 5, NgayThongKe = "05/2024", SoLuong = 200 },
                new ItemThongKeNgay { STT = 6, NgayThongKe = "06/2024", SoLuong = 176 },
                new ItemThongKeNgay { STT = 7, NgayThongKe = "07/2024", SoLuong = 120 },
                new ItemThongKeNgay { STT = 8, NgayThongKe = "08/2024", SoLuong = 100 },
                new ItemThongKeNgay { STT = 9, NgayThongKe = "09/2024", SoLuong = 234 },
                new ItemThongKeNgay { STT = 10, NgayThongKe = "10//2024", SoLuong = 190 },
                new ItemThongKeNgay { STT = 11, NgayThongKe = "11/2024", SoLuong = 140 },
                new ItemThongKeNgay { STT = 12, NgayThongKe = "12/2024", SoLuong = 140 }
            };

            listViewThongKeNgay.ItemsSource = data;
        }

        private void LoadDataThang()
        {
            var data = new List<ItemThongKeThang>
            {
                new ItemThongKeThang { STT = 1, NgayThongKe = "01/10/2024", SoLuong = 240 },
                new ItemThongKeThang { STT = 2, NgayThongKe = "02/10/2024", SoLuong = 255 },
                new ItemThongKeThang { STT = 3, NgayThongKe = "03/10/2024", SoLuong = 180 },
                new ItemThongKeThang { STT = 4, NgayThongKe = "04/10/2024", SoLuong = 233 },
                new ItemThongKeThang { STT = 5, NgayThongKe = "05/10/2024", SoLuong = 200 },
                new ItemThongKeThang { STT = 6, NgayThongKe = "06/10/2024", SoLuong = 176 },
                new ItemThongKeThang { STT = 7, NgayThongKe = "07/10/2024", SoLuong = 120 },
                new ItemThongKeThang { STT = 8, NgayThongKe = "08/10/2024", SoLuong = 100 },
                new ItemThongKeThang { STT = 9, NgayThongKe = "09/10/2024", SoLuong = 234 },
                new ItemThongKeThang { STT = 10, NgayThongKe = "10/10/2024", SoLuong = 190 },
                new ItemThongKeThang { STT = 11, NgayThongKe = "11/10/2024", SoLuong = 140 }
            };

            listViewThongKeThang.ItemsSource = data;
        }

        public class ItemThongKeNgay
        {
            public int STT { get; set; }
            public string NgayThongKe { get; set; }
            public int SoLuong { get; set; }
        }

        public class ItemThongKeThang
        {
            public int STT { get; set; }
            public string NgayThongKe { get; set; }
            public int SoLuong { get; set; }
        }

        private void btnBieuDo_Click(object sender, RoutedEventArgs e)
        {
            isBieuDoClicked = true;
            CheckAndShowOverlayChart();
        }

        private void RabtnSoLuong_Checked(object sender, RoutedEventArgs e)
        {
            isSoLuongChecked = true;
            CheckAndShowOverlayChart();
        }
        private void CheckAndShowOverlayChart()
        {
            // Kiểm tra 3 điều kiện cùng lúc
            if (isThongKeClicked && isBieuDoClicked && isSoLuongChecked)
            {
                OverlayChartSoLuongNgay.Visibility = Visibility.Visible;
            }
        }

        private void RabtnDoanhThu_Checked(object sender, RoutedEventArgs e)
        {
            isSoLuongChecked = true;
            CheckAndShowOverlayChart();
        }

        private void RabtnUaThich_Checked(object sender, RoutedEventArgs e)
        {
            isSoLuongChecked = true;
            CheckAndShowOverlayChart();
        }

        private void RabtnLuong_Checked(object sender, RoutedEventArgs e)
        {
            isSoLuongChecked = true;
            CheckAndShowOverlayChart();
        }
    }
}
