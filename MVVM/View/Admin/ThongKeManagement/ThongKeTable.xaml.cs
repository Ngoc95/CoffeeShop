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

        private bool isThongKeNgay = false; // Đánh dấu đang chọn Thống kê theo Ngày
        private void ThongKeDay_Button_Click(object sender, RoutedEventArgs e)
        {
            isThongKeNgay = true;
            btnThongKeNgay.Background = new SolidColorBrush(Color.FromArgb(255, 240, 153, 125));
            btnThongKeThang.Background = new SolidColorBrush(Colors.White);

        }

        private void ThongKeThang_Button_Click(object sender, RoutedEventArgs e)
        {
            isThongKeNgay = false;
            btnThongKeThang.Background = new SolidColorBrush(Color.FromArgb(255, 240, 153, 125));
            btnThongKeNgay.Background = new SolidColorBrush(Colors.White);


        }

        private void btnDanhSach_Click(object sender, RoutedEventArgs e)
        {
            // Ẩn tất cả các OverlayGrid trước
            HideAllOverlays();

            // Hiển thị Overlay tương ứng dựa trên trạng thái và RadioButton được chọn
            if (isThongKeNgay)
            {
                if (RabtnSoLuong.IsChecked == true)
                    OverlayGridDSSoLuongNgay.Visibility = Visibility.Visible;
                else if (RabtnDoanhThu.IsChecked == true)
                    OverlayGridDSDoanhThuNgay.Visibility = Visibility.Visible;
                else if (RabtnUaThich.IsChecked == true)
                    OverlayGridDSUaThichNgay.Visibility = Visibility.Visible;
                else if (RabtnLuong.IsChecked == true)
                    OverlayGridDSLuongNgay.Visibility = Visibility.Visible;
            }
            else
            {
                if (RabtnSoLuong.IsChecked == true)
                    OverlayGridDSSoLuongThang.Visibility = Visibility.Visible;
                else if (RabtnDoanhThu.IsChecked == true)
                    OverlayGridDSDoanhThuThang.Visibility = Visibility.Visible;
                else if (RabtnUaThich.IsChecked == true)
                    OverlayGridDSUaThichThang.Visibility = Visibility.Visible;
                else if (RabtnLuong.IsChecked == true)
                    OverlayGridDSLuongThang.Visibility = Visibility.Visible;
            }
        }



        private void btnXuatExcelOverlay_Click(object sender, RoutedEventArgs e)
        {
            // Code xử lý xuất Excel
            MessageBox.Show("Xuất dữ liệu ra Excel!");
            OverlayGridDSSoLuongNgay.Visibility = Visibility.Collapsed;
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

            listViewSoLuongNgay.ItemsSource = data;
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

            listViewSoLuongThang.ItemsSource = data;
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
            // Ẩn tất cả các OverlayGrid trước
            HideAllOverlays();

            // Hiển thị Overlay tương ứng dựa trên trạng thái và RadioButton được chọn
            if (isThongKeNgay)
            {
                if (RabtnSoLuong.IsChecked == true)
                    OverlayChartSoLuongNgay.Visibility = Visibility.Visible;
                else if (RabtnDoanhThu.IsChecked == true)
                    OverlayChartDoanhThuNgay.Visibility = Visibility.Visible;
                else if (RabtnUaThich.IsChecked == true)
                    OverlayChartUaThichNgay.Visibility = Visibility.Visible;
                else if (RabtnLuong.IsChecked == true)
                    OverlayChartLuongNgay.Visibility = Visibility.Visible;
            }
            else
            {
                if (RabtnSoLuong.IsChecked == true)
                    OverlayChartSoLuongThang.Visibility = Visibility.Visible;
                else if (RabtnDoanhThu.IsChecked == true)
                    OverlayChartDoanhThuThang.Visibility = Visibility.Visible;
                else if (RabtnUaThich.IsChecked == true)
                    OverlayChartUaThichThang.Visibility = Visibility.Visible;
                else if (RabtnLuong.IsChecked == true)
                    OverlayChartLuongThang.Visibility = Visibility.Visible;
            }
        }

        private void HideAllOverlays()
        {
            OverlayGridDSSoLuongNgay.Visibility = Visibility.Collapsed;
            OverlayGridDSDoanhThuNgay.Visibility = Visibility.Collapsed;
            OverlayGridDSUaThichNgay.Visibility = Visibility.Collapsed;
            OverlayGridDSLuongNgay.Visibility = Visibility.Collapsed;

            OverlayGridDSSoLuongThang.Visibility = Visibility.Collapsed;
            OverlayGridDSDoanhThuThang.Visibility = Visibility.Collapsed;
            OverlayGridDSUaThichThang.Visibility = Visibility.Collapsed;
            OverlayGridDSLuongThang.Visibility = Visibility.Collapsed;

            OverlayChartSoLuongNgay.Visibility = Visibility.Collapsed;
            OverlayChartDoanhThuNgay.Visibility = Visibility.Collapsed;
            OverlayChartUaThichNgay.Visibility = Visibility.Collapsed;
            OverlayChartLuongNgay.Visibility = Visibility.Collapsed;

            OverlayChartSoLuongThang.Visibility = Visibility.Collapsed;
            OverlayChartDoanhThuThang.Visibility = Visibility.Collapsed;
            OverlayChartUaThichThang.Visibility = Visibility.Collapsed;
            OverlayChartLuongThang.Visibility = Visibility.Collapsed;
        }

    }
}
