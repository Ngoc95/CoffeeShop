using QuanLiCoffeeShop.MVVM.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement
{
    /// <summary>
    /// Interaction logic for DieuChinhCaLamViecWindow.xaml
    /// </summary>
    public partial class DieuChinhCaLamViecWindow : Window
    {
        public DieuChinhCaLamViecWindow()
        {
            InitializeComponent();
        }

        private void moveChangeCaWin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CaToi_Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (WorkshiftViewModel)this.DataContext;
            viewModel?.LoadShiftCM.Execute(null);
            btnCaToi.Background = new SolidColorBrush(Colors.White);
            btnCaSang.Background = new SolidColorBrush(Color.FromRgb(255, 195, 161));
            btnCaChieu.Background = new SolidColorBrush(Color.FromRgb(255, 195, 161));
            OverlayGridCaToi.Visibility = Visibility.Visible;
            OverlayGridCaSang.Visibility = Visibility.Collapsed;
            OverlayGridCaChieu.Visibility = Visibility.Collapsed;
        }

        private void CaChieu_Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (WorkshiftViewModel)this.DataContext;
            viewModel?.LoadShiftCM.Execute(null);
            btnCaChieu.Background = new SolidColorBrush(Colors.White);
            btnCaSang.Background = new SolidColorBrush(Color.FromRgb(255, 195, 161));
            btnCaToi.Background = new SolidColorBrush(Color.FromRgb(255, 195, 161));
            OverlayGridCaChieu.Visibility = Visibility.Visible;
            OverlayGridCaSang.Visibility = Visibility.Collapsed;
            OverlayGridCaToi.Visibility = Visibility.Collapsed;
        }

        private void CaSang_Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (WorkshiftViewModel)this.DataContext;
            viewModel?.LoadShiftCM.Execute(null);
            btnCaSang.Background = new SolidColorBrush(Colors.White);
            btnCaChieu.Background = new SolidColorBrush(Color.FromRgb(255, 195, 161));
            btnCaToi.Background = new SolidColorBrush(Color.FromRgb(255, 195, 161));
            OverlayGridCaSang.Visibility = Visibility.Visible;
            OverlayGridCaChieu.Visibility = Visibility.Collapsed;
            OverlayGridCaToi.Visibility = Visibility.Collapsed;
        }

        private void txtTienLuong_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Chỉ cho phép nhập các ký tự số (từ 0 đến 9)
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }

        private void txtTienLuong_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lấy giá trị TextBox bỏ đi phần "đ" và khoảng trắng
            string input = txtTienLuongCaSang.Text.Replace("đ", "").Trim();

            // Kiểm tra và chỉ format nếu input là số hợp lệ
            if (decimal.TryParse(input, out decimal tienLuong))
            {
                txtTienLuongCaSang.Text = $"{tienLuong:#,##0} đ"; // Hiển thị lại với định dạng "số + đ"
                txtTienLuongCaSang.CaretIndex = txtTienLuongCaSang.Text.Length; // Đặt con trỏ ở cuối
            }
            else
            {
                // Trường hợp nhập không phải là số thì đặt lại giá trị
                txtTienLuongCaSang.Text = "0 đ";
                txtTienLuongCaSang.CaretIndex = txtTienLuongCaSang.Text.Length;
            }
        }

        private void BoQua_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

